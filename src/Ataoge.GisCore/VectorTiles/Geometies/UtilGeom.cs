using System.Collections.Generic;

namespace Ataoge.GisCore.VectorTiles.Geometries
{
    /// <summary>
	/// Geometry related helper methods
	/// </summary>
	public static class UtilGeom
	{


		/// <summary>
		/// TO BE REMOVED!!! Processing geometries is out of scope. 
		/// Clip geometries extending beyond the tile border.
		/// </summary>
		/// <param name="geoms">Raw tile geometries of the feature</param>
		/// <param name="geomType">Geometry type of the feature</param>
		/// <param name="extent">Extent of the layer </param>
		/// <param name="bufferSize">Units (in internal tile coordinates) to go beyond the tile border. Pass '0' to clip exactly at the tile border</param>
		/// <param name="scale">Factor for scaling the geometries</param>
		/// <returns></returns>
		public static List<List<Point2d<long>>> ClipGeometries(
			List<List<Point2d<long>>> geoms
			, GeomType geomType
			, long extent
			, uint bufferSize
			, float scale
			)
		{
            List<List<Point2d<long>>> retVal = new List<List<Point2d<long>>>();

            return null;
        }
    }
}