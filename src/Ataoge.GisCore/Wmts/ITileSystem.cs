namespace Ataoge.GisCore.Wmts
{
   
    public interface ITileSystem
    {
        double EarthRadius { get; }
        double InitialResolution { get; }
        double OriginXShift { get; }
        double OriginYShift { get; }

        double GetMapScale(int zoom, int screenDpi = 96);
        double GetMapScale(double latitude, int zoom, int screenDpi = 96);
        double GetResolution(int zoom);
        double GetResolution(double latitude, int zoom);
        
        uint MapSize(int zoom);

        PixelPoint PixelToRaster(int px, int py, int zoom);
        PixelPoint PixelToTile(int px, int py, int bTopRight = 0);

        PixelPoint LatLonToPixel(double lat, double lon, int zoom);

        PixelPoint LatLonToTile(double lat, double lon, int zoom, int bTopRight = 0);

        MapPoint PixelToLatLon(int px, int py, int zoom);

        MapExtent TileToLatLonBounds(int tx, int ty, int zoom);

        TileExtent LatLonBoundsToTileMatrix(double minLat, double minLon, double maxLat, double maxLon, int zoom);

        PixelPoint TileToPixel(int tx, int ty);

        int ZoomForPixelSize(double pixelSize);

        TileSize GetTileMatrix(int zoom);

    }

    public interface IProjectedTileSystem
    {
        MapPoint PixelToXY(int px, int py, int zoom);
        MapPoint LatLonToXY(double lat, double lon);
        MapPoint XYToLatLon(double x, double y);
        MapExtent TileToXYBounds(int tx, int ty, int zoom);
        PixelPoint XYToPixel(double x, double y, int zoom);
        PixelPoint XYToTile(double x, double y, int zoom, int bTopRight = 0);
        TileExtent XYBoundsToTileMatrix(double minX, double minY, double maxX, double maxY, int zoom);
   }


}