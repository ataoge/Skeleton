namespace Ataoge.GisCore
{
    public enum LabelPlacement
    {
        esriServerPointLabelPlacementAboveCenter,
        esriServerPointLabelPlacementBelowCenter,
        esriServerPointLabelPlacementCenterCenter,
        esriServerPointLabelPlacementAboveLeft,
        esriServerPointLabelPlacementBelowLeft,
        esriServerPointLabelPlacementCenterLeft,
        esriServerPointLabelPlacementAboveRight,
        esriServerPointLabelPlacementBelowRight,
        esriServerPointLabelPlacementCenterRight,
        esriServerLinePlacementAboveAfter,
        esriServerLinePlacementAboveAlong,
        esriServerLinePlacementAboveBefore,
        esriServerLinePlacementAboveStart,
        esriServerLinePlacementAboveEnd,
        esriServerLinePlacementBelowAfter,
        esriServerLinePlacementBelowAlong,
        esriServerLinePlacementBelowBefore,
        esriServerLinePlacementBelowStart,
        esriServerLinePlacementBelowEnd,
        esriServerLinePlacementCenterAfter,
        esriServerLinePlacementCenterAlong,
        esriServerLinePlacementCenterBefore,
        esriServerLinePlacementCenterStart,
        esriServerLinePlacementCenterEnd,
        esriServerPolygonPlacementAlwaysHorizontal
    }

    public class Label
    {
        public LabelPlacement LabelPlacement {get; set;}

        public string LabelExpression {get; set;}

        public bool UseCodeValues {get; set;}

        public TextSymbol Symbol {get; set;}

        public double minScale {get; set;}

        public double maxScale {get; set;}
    }
}