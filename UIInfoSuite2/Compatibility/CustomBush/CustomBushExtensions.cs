using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;
using StardewValley.ItemTypeDefinitions;
using StardewValley.TerrainFeatures;
using UIInfoSuite2.Infrastructure;

namespace UIInfoSuite2.Compatibility.CustomBush;

public record CustomBushDroppedItem(string BushId, int NextDayToProduce, ParsedItemData Item, float Chance)
{
  public bool ReadyToPick => Game1.dayOfMonth == NextDayToProduce;
}

internal static class CustomBushExtensions
{
  public static List<CustomBushDroppedItem> GetCustomBushDropItems(
    this ICustomBushApi api,
    Bush bush,
    ICustomBush customBush,
    bool includeToday = true
  )
  {
    List<CustomBushDroppedItem> items = new();

    if (!api.TryGetDrops(bush, out List<ICustomBushDrop>? drops))
    {
      return items;
    }

    foreach (ICustomBushDrop drop in drops)
    {
      int? nextDay = string.IsNullOrEmpty(drop.Condition)
        ? Game1.dayOfMonth + (includeToday ? 0 : 1)
        : Tools.GetNextDayFromCondition(drop.Condition, includeToday);
      int? lastDay = Tools.GetLastDayFromCondition(drop.Condition);
      // TODO this assumes that the only item in drop is ItemId. If RandomItemId is used, this will not work.
      ParsedItemData? itemData = ItemRegistry.GetData(drop.ItemId);
      if (!nextDay.HasValue)
      {
        if (!lastDay.HasValue)
        {
          ModEntry.MonitorObject.Log(
            $"Couldn't parse the next day the bush {customBush.DisplayName} will drop {drop.ItemId}. Condition: {drop.Condition}. Please report this error.",
            LogLevel.Error
          );
        }

        continue;
      }

      if (itemData == null)
      {
        ModEntry.MonitorObject.Log(
          $"Couldn't parse the correct item {customBush.DisplayName} will drop. ItemId: {drop.ItemId}. Please report this error.",
          LogLevel.Error
        );
        continue;
      }

      if (Game1.dayOfMonth == nextDay.Value && !includeToday)
      {
        continue;
      }

      items.Add(new CustomBushDroppedItem(customBush.Id, nextDay.Value, itemData, drop.Chance));
    }

    return items;
  }
}
