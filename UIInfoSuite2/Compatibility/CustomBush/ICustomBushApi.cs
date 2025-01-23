using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace UIInfoSuite2.Compatibility.CustomBush;

/// <summary>Mod API for custom bushes.</summary>
public interface ICustomBushApi
{
  /// <summary>Try to get the custom bush model associated with the given bush.</summary>
  /// <param name="bush">The bush to check.</param>
  /// <param name="customBush">The resulting custom bush, if applicable.</param>
  /// <returns>Returns whether a custom bush was found.</returns>
  public bool TryGetBush(Bush bush, [NotNullWhen(true)] out ICustomBush? customBush);

  /// <summary>Try to get the custom bush drop associated with the given bush id.</summary>
  /// <param name="bush">The bush to check.</param>
  /// <param name="drops">The items produced by the custom bush, if applicable.</param>
  /// <returns>Returns whether custom bush drops were found.</returns>
  public bool TryGetDrops(Bush bush, [NotNullWhen(true)] out List<ICustomBushDrop>? drops);

  /// <summary>Try to get the shake off item.</summary>
  /// <param name="bush">The bush.</param>
  /// <param name="item">The shake off item.</param>
  /// <returns>Returns True if the custom bush currently has an item to collect.</returns>
  public bool TryGetShakeOffItem(Bush bush, [NotNullWhen(true)] out Item? item);
}
