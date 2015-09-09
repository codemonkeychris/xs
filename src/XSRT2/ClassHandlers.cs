//  #define USE_LISTVIEW_VIRTUALIZATION
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Text;
using System.Reflection;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;

namespace XSRT2 {
    public static class Handler
    {
        internal static partial class MapItemsControlHandler
        {
            internal static MapItemsControl Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<MapItemsControl>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(MapItemsControl t, JObject obj, JObject lastObj, DiffContext context)
            {
                TrySet(context, obj, lastObj,
                    "items", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { SetItems(target, valueToken, lastValueToken, context); });
            }
        }
                
        internal static partial class MapElementHandler
        {
            internal static void SetProperties(MapElement t, JObject obj, JObject lastObj, DiffContext context)
            {
            }
        }
                
        internal static partial class MapPolygonHandler
        {
            internal static MapPolygon Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<MapPolygon>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(MapPolygon t, JObject obj, JObject lastObj, DiffContext context)
            {
                MapElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "fillColor", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FillColor = XamlStringParse<Color>(valueToken));
                TrySet(context, obj, lastObj,
                    "strokeColor", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.StrokeColor = XamlStringParse<Color>(valueToken));
                TrySet(context, obj, lastObj,
                    "path", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => {
                    JArray valueObj = (JArray)valueToken;
                    var path = new List<BasicGeoposition>();
                    foreach (var posToken in valueObj.AsJEnumerable().OfType<JObject>())
                    {
                        path.Add(new BasicGeoposition() {
                            Latitude = posToken["latitude"].Value<double>(),
                            Longitude = posToken["longitude"].Value<double>()
                        });
                    }
                
                    target.Path = new Geopath(path);
                });
            }
        }
                
        internal static partial class MapControlHandler
        {
            internal static MapControl Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<MapControl>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                SetChildren(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                SetMapElements(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(MapControl t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "businessLandmarksVisible", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.BusinessLandmarksVisible = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "center", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => {
                    JObject valueObj = (JObject)valueToken;
                    BasicGeoposition pos = new BasicGeoposition() {
                        Latitude = valueObj["latitude"].Value<double>(),
                        Longitude = valueObj["longitude"].Value<double>()
                    };
                    target.Center = new Geopoint(pos);
                });
                TrySet(context, obj, lastObj,
                    "desiredPitch", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.DesiredPitch = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "heading", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Heading = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "landmarksVisible", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.LandmarksVisible = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "mapServiceToken", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.MapServiceToken = valueToken.ToString());
                TrySet(context, obj, lastObj,
                    "pedestrianFeaturesVisible", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.PedestrianFeaturesVisible = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "trafficFlowVisible", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TrafficFlowVisible = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "transitFeaturesVisible", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TransitFeaturesVisible = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "zoomLevel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.ZoomLevel = valueToken.Value<double>());
                TrySetEvent(context, obj, lastObj, "ActualCameraChanged", t, (target, valueToken, lastValueToken) => SetActualCameraChangedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "ActualCameraChanging", t, (target, valueToken, lastValueToken) => SetActualCameraChangingEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "CenterChanged", t, (target, valueToken, lastValueToken) => SetCenterChangedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "CustomExperienceChanged", t, (target, valueToken, lastValueToken) => SetCustomExperienceChangedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "HeadingChanged", t, (target, valueToken, lastValueToken) => SetHeadingChangedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "LoadingStatusChanged", t, (target, valueToken, lastValueToken) => SetLoadingStatusChangedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "MapDoubleTapped", t, (target, valueToken, lastValueToken) => SetMapDoubleTappedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "MapElementClick", t, (target, valueToken, lastValueToken) => SetMapElementClickEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "MapElementPointerEntered", t, (target, valueToken, lastValueToken) => SetMapElementPointerEnteredEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "MapElementPointerExited", t, (target, valueToken, lastValueToken) => SetMapElementPointerExitedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "MapHolding", t, (target, valueToken, lastValueToken) => SetMapHoldingEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "MapTapped", t, (target, valueToken, lastValueToken) => SetMapTappedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "PitchChanged", t, (target, valueToken, lastValueToken) => SetPitchChangedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "TargetCameraChanged", t, (target, valueToken, lastValueToken) => SetTargetCameraChangedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "TransformOriginChanged", t, (target, valueToken, lastValueToken) => SetTransformOriginChangedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "ZoomLevelChanged", t, (target, valueToken, lastValueToken) => SetZoomLevelChangedEventHandler(valueToken.ToString(), target));
            }
            static void ActualCameraChangedRouter(object sender, MapActualCameraChangedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["ActualCameraChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetActualCameraChangedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["ActualCameraChanged"] = handlerName;
                element.ActualCameraChanged -= ActualCameraChangedRouter;
                element.ActualCameraChanged += ActualCameraChangedRouter;
            }
            static void ActualCameraChangingRouter(object sender, MapActualCameraChangingEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["ActualCameraChanging"], Sender = sender, EventArgs = e });
                }
            }
            static void SetActualCameraChangingEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["ActualCameraChanging"] = handlerName;
                element.ActualCameraChanging -= ActualCameraChangingRouter;
                element.ActualCameraChanging += ActualCameraChangingRouter;
            }
            static void CenterChangedRouter(object sender, Object e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["CenterChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetCenterChangedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["CenterChanged"] = handlerName;
                element.CenterChanged -= CenterChangedRouter;
                element.CenterChanged += CenterChangedRouter;
            }
            static void CustomExperienceChangedRouter(object sender, MapCustomExperienceChangedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["CustomExperienceChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetCustomExperienceChangedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["CustomExperienceChanged"] = handlerName;
                element.CustomExperienceChanged -= CustomExperienceChangedRouter;
                element.CustomExperienceChanged += CustomExperienceChangedRouter;
            }
            static void HeadingChangedRouter(object sender, Object e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["HeadingChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetHeadingChangedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["HeadingChanged"] = handlerName;
                element.HeadingChanged -= HeadingChangedRouter;
                element.HeadingChanged += HeadingChangedRouter;
            }
            static void LoadingStatusChangedRouter(object sender, Object e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["LoadingStatusChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetLoadingStatusChangedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["LoadingStatusChanged"] = handlerName;
                element.LoadingStatusChanged -= LoadingStatusChangedRouter;
                element.LoadingStatusChanged += LoadingStatusChangedRouter;
            }
            static void MapDoubleTappedRouter(object sender, MapInputEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["MapDoubleTapped"], Sender = sender, EventArgs = e });
                }
            }
            static void SetMapDoubleTappedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["MapDoubleTapped"] = handlerName;
                element.MapDoubleTapped -= MapDoubleTappedRouter;
                element.MapDoubleTapped += MapDoubleTappedRouter;
            }
            static void MapElementClickRouter(object sender, MapElementClickEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["MapElementClick"], Sender = sender, EventArgs = e });
                }
            }
            static void SetMapElementClickEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["MapElementClick"] = handlerName;
                element.MapElementClick -= MapElementClickRouter;
                element.MapElementClick += MapElementClickRouter;
            }
            static void MapElementPointerEnteredRouter(object sender, MapElementPointerEnteredEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["MapElementPointerEntered"], Sender = sender, EventArgs = e });
                }
            }
            static void SetMapElementPointerEnteredEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["MapElementPointerEntered"] = handlerName;
                element.MapElementPointerEntered -= MapElementPointerEnteredRouter;
                element.MapElementPointerEntered += MapElementPointerEnteredRouter;
            }
            static void MapElementPointerExitedRouter(object sender, MapElementPointerExitedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["MapElementPointerExited"], Sender = sender, EventArgs = e });
                }
            }
            static void SetMapElementPointerExitedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["MapElementPointerExited"] = handlerName;
                element.MapElementPointerExited -= MapElementPointerExitedRouter;
                element.MapElementPointerExited += MapElementPointerExitedRouter;
            }
            static void MapHoldingRouter(object sender, MapInputEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["MapHolding"], Sender = sender, EventArgs = e });
                }
            }
            static void SetMapHoldingEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["MapHolding"] = handlerName;
                element.MapHolding -= MapHoldingRouter;
                element.MapHolding += MapHoldingRouter;
            }
            static void MapTappedRouter(object sender, MapInputEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["MapTapped"], Sender = sender, EventArgs = e });
                }
            }
            static void SetMapTappedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["MapTapped"] = handlerName;
                element.MapTapped -= MapTappedRouter;
                element.MapTapped += MapTappedRouter;
            }
            static void PitchChangedRouter(object sender, Object e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["PitchChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetPitchChangedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["PitchChanged"] = handlerName;
                element.PitchChanged -= PitchChangedRouter;
                element.PitchChanged += PitchChangedRouter;
            }
            static void TargetCameraChangedRouter(object sender, MapTargetCameraChangedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["TargetCameraChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetTargetCameraChangedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["TargetCameraChanged"] = handlerName;
                element.TargetCameraChanged -= TargetCameraChangedRouter;
                element.TargetCameraChanged += TargetCameraChangedRouter;
            }
            static void TransformOriginChangedRouter(object sender, Object e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["TransformOriginChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetTransformOriginChangedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["TransformOriginChanged"] = handlerName;
                element.TransformOriginChanged -= TransformOriginChangedRouter;
                element.TransformOriginChanged += TransformOriginChangedRouter;
            }
            static void ZoomLevelChangedRouter(object sender, Object e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["ZoomLevelChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetZoomLevelChangedEventHandler(string handlerName, MapControl element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["ZoomLevelChanged"] = handlerName;
                element.ZoomLevelChanged -= ZoomLevelChangedRouter;
                element.ZoomLevelChanged += ZoomLevelChangedRouter;
            }
        }

        internal static partial class ShapeHandler
        {
            internal static void SetProperties(Shape t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "fill", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Fill = XamlStringParse<Brush>(valueToken));
                TrySet(context, obj, lastObj,
                    "stroke", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Stroke = XamlStringParse<Brush>(valueToken));
                TrySet(context, obj, lastObj,
                    "strokeThickness", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.StrokeThickness = valueToken.Value<double>());
            }
        }

        internal static partial class EllipseHandler
        {
            internal static Ellipse Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Ellipse>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(Ellipse t, JObject obj, JObject lastObj, DiffContext context)
            {
                ShapeHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static partial class RectangleHandler
        {
            internal static Rectangle Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Rectangle>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(Rectangle t, JObject obj, JObject lastObj, DiffContext context)
            {
                ShapeHandler.SetProperties(t, obj, lastObj, context);
            }
        }



        internal static partial class UIElementHandler
        {
            internal static void SetProperties(UIElement t, JObject obj, JObject lastObj, DiffContext context)
            {
                TrySet(context, obj, lastObj,
                    "opacity", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Opacity = valueToken.Value<double>());
                TrySetEvent(context, obj, lastObj, "KeyDown", t, (target, valueToken, lastValueToken) => SetKeyDownEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "KeyUp", t, (target, valueToken, lastValueToken) => SetKeyUpEventHandler(valueToken.ToString(), target));
            }
            static void KeyDownRouter(object sender, KeyRoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["KeyDown"], Sender = sender, EventArgs = e });
                }
            }
            static void SetKeyDownEventHandler(string handlerName, UIElement element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["KeyDown"] = handlerName;
                element.KeyDown -= KeyDownRouter;
                element.KeyDown += KeyDownRouter;
            }
            static void KeyUpRouter(object sender, KeyRoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["KeyUp"], Sender = sender, EventArgs = e });
                }
            }
            static void SetKeyUpEventHandler(string handlerName, UIElement element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["KeyUp"] = handlerName;
                element.KeyUp -= KeyUpRouter;
                element.KeyUp += KeyUpRouter;
            }
        }

        internal static partial class FrameworkElementHandler
        {
            internal static void SetProperties(FrameworkElement t, JObject obj, JObject lastObj, DiffContext context)
            {
                UIElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "scrollViewer$verticalScrollBarVisibility", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, XamlStringParse<ScrollBarVisibility>(valueToken)); });
                TrySet(context, obj, lastObj,
                    "scrollViewer$horizontalScrollBarVisibility", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, XamlStringParse<ScrollBarVisibility>(valueToken)); });
                TrySet(context, obj, lastObj,
                    "map$location", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => {
                    JObject valueObj = (JObject)valueToken;
                    BasicGeoposition pos = new BasicGeoposition() {
                        Latitude = valueObj["latitude"].Value<double>(),
                        Longitude = valueObj["longitude"].Value<double>()
                    };
                    target.SetValue(MapControl.LocationProperty, new Geopoint(pos));
                });
                TrySet(context, obj, lastObj,
                    "grid$row", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(Grid.RowProperty, Convert.ToInt32(valueToken.Value<double>())); });
                TrySet(context, obj, lastObj,
                    "grid$rowSpan", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(Grid.RowSpanProperty, Convert.ToInt32(valueToken.Value<double>())); });
                TrySet(context, obj, lastObj,
                    "grid$column", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(Grid.ColumnProperty, Convert.ToInt32(valueToken.Value<double>())); });
                TrySet(context, obj, lastObj,
                    "grid$columnSpan", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(Grid.ColumnSpanProperty, Convert.ToInt32(valueToken.Value<double>())); });
                TrySet(context, obj, lastObj,
                    "automationId", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { AutomationProperties.SetAutomationId(target, valueToken.ToString()); });
                TrySet(context, obj, lastObj,
                    "acc$helpText", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { AutomationProperties.SetHelpText(target, valueToken.ToString()); });
                TrySet(context, obj, lastObj,
                    "acc$labeledBy", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(AutomationProperties.LabeledByProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "acc$liveSetting", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(AutomationProperties.LiveSettingProperty, ParseEnum<AutomationLiveSetting>(valueToken)));
                TrySet(context, obj, lastObj,
                    "acc$name", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { AutomationProperties.SetName(target, valueToken.ToString()); });
                TrySet(context, obj, lastObj,
                    "relative$above", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AboveProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignBottomWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignBottomWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignBottomWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignBottomWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$alignHorizontalCenterWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignHorizontalCenterWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignLeftWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignLeftWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignLeftWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignLeftWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$alignRightWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignRightWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignRightWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignRightWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$alignTopWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignTopWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignTopWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignTopWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$alignVerticalCenterWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignVerticalCenterWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignVerticalCenterWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignVerticalCenterWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$below", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.BelowProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$leftOf", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.LeftOfProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$rightOf", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.RightOfProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "horizontalAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalAlignment = ParseEnum<HorizontalAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "verticalAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalAlignment = ParseEnum<VerticalAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "margin", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Margin = XamlStringParse<Thickness>(valueToken));
                TrySet(context, obj, lastObj,
                    "width", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Width = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "height", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Height = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "name", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.Name = valueToken.ToString(); });
            }
        }

        internal static partial class ViewboxHandler
        {
            internal static Viewbox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Viewbox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(Viewbox t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "child", 
                    true,
                    t,
                    (target, valueToken, lastValueToken) => target.Child = (UIElement)CreateFromState(valueToken, lastValueToken, context));
                TrySet(context, obj, lastObj,
                    "stretch", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Stretch = XamlStringParse<Stretch>(valueToken));
                TrySet(context, obj, lastObj,
                    "stretchDirection", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.StretchDirection = XamlStringParse<StretchDirection>(valueToken));
            }
        }
        internal static partial class TextBlockHandler
        {
            internal static TextBlock Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<TextBlock>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(TextBlock t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "foreground", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Foreground = XamlStringParse<Brush>(valueToken));
                TrySet(context, obj, lastObj,
                    "text", 
                    true,
                    t,
                    (target, valueToken, lastValueToken) => target.Text = valueToken.ToString());
                TrySet(context, obj, lastObj,
                    "fontFamily", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontFamily = new FontFamily(valueToken.ToString()));
                TrySet(context, obj, lastObj,
                    "fontSize", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontSize = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "fontWeight", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontWeight = XamlStringParse<FontWeight>(valueToken));
            }
        }

        internal static partial class ImageHandler
        {
            internal static Image Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Image>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(Image t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "source", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Source = new BitmapImage(FromRelativeUri(target, valueToken.ToString(), context)), context.Defer);
            }
        }
        
        internal static partial class RichEditBoxHandler
        {
            internal static RichEditBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<RichEditBox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(RichEditBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "acceptsReturn", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.AcceptsReturn = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "text", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Document.SetText(TextSetOptions.None, valueToken.ToString()));
                TrySet(context, obj, lastObj,
                    "isColorFontEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsColorFontEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isReadOnly", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsReadOnly = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isSpellCheckEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsSpellCheckEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isTextPredictionEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsTextPredictionEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "placeholderText", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.PlaceholderText = valueToken.ToString());
                TrySet(context, obj, lastObj,
                    "preventKeyboardDisplayOnProgrammaticFocus", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.PreventKeyboardDisplayOnProgrammaticFocus = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "textAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextAlignment = ParseEnum<TextAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "textReadingOrder", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextReadingOrder = ParseEnum<TextReadingOrder>(valueToken));
                TrySet(context, obj, lastObj,
                    "textWrapping", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextWrapping = ParseEnum<TextWrapping>(valueToken));
                TrySetEvent(context, obj, lastObj, "TextChanged", t, (target, valueToken, lastValueToken) => SetTextChangedEventHandler(valueToken.ToString(), target));
            }
            static void TextChangedRouter(object sender, RoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["TextChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetTextChangedEventHandler(string handlerName, RichEditBox element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["TextChanged"] = handlerName;
                element.TextChanged -= TextChangedRouter;
                element.TextChanged += TextChangedRouter;
            }
        }

        internal static partial class TextBoxHandler
        {
            internal static TextBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<TextBox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(TextBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "acceptsReturn", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.AcceptsReturn = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "text", 
                    true,
                    t,
                    (target, valueToken, lastValueToken) => target.Text = valueToken.ToString());
                TrySet(context, obj, lastObj,
                    "isColorFontEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsColorFontEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isReadOnly", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsReadOnly = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isSpellCheckEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsSpellCheckEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isTextPredictionEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsTextPredictionEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "maxLength", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.MaxLength = valueToken.Value<int>());
                TrySet(context, obj, lastObj,
                    "placeholderText", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.PlaceholderText = valueToken.ToString());
                TrySet(context, obj, lastObj,
                    "preventKeyboardDisplayOnProgrammaticFocus", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.PreventKeyboardDisplayOnProgrammaticFocus = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "selectionLength", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SelectionLength = valueToken.Value<int>());
                TrySet(context, obj, lastObj,
                    "selectionStart", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SelectionStart = valueToken.Value<int>());
                TrySet(context, obj, lastObj,
                    "textAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextAlignment = ParseEnum<TextAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "textReadingOrder", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextReadingOrder = ParseEnum<TextReadingOrder>(valueToken));
                TrySet(context, obj, lastObj,
                    "textWrapping", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextWrapping = ParseEnum<TextWrapping>(valueToken));
                TrySetEvent(context, obj, lastObj, "TextChanged", t, (target, valueToken, lastValueToken) => SetTextChangedEventHandler(valueToken.ToString(), target));
            }
            static void TextChangedRouter(object sender, RoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["TextChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetTextChangedEventHandler(string handlerName, TextBox element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["TextChanged"] = handlerName;
                element.TextChanged -= TextChangedRouter;
                element.TextChanged += TextChangedRouter;
            }
        }

        internal static partial class GridViewHandler
        {
            internal static GridView Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<GridView>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(GridView t, JObject obj, JObject lastObj, DiffContext context)
            {
                ListViewBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }

        internal static partial class ListViewHandler
        {
            internal static ListView Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ListView>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                AddListViewFixups(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(ListView t, JObject obj, JObject lastObj, DiffContext context)
            {
                ListViewBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }

        internal static partial class ListViewBaseHandler
        {
            internal static void SetProperties(ListViewBase t, JObject obj, JObject lastObj, DiffContext context)
            {
                SelectorHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        
        internal static partial class ComboBoxHandler
        {
            internal static ComboBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ComboBox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(ComboBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                SelectorHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        
        internal static partial class ListBoxHandler
        {
            internal static ListBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ListBox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(ListBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                SelectorHandler.SetProperties(t, obj, lastObj, context);
            }
        }

        internal static partial class SelectorHandler
        {
            internal static void SetProperties(Selector t, JObject obj, JObject lastObj, DiffContext context)
            {
                ItemsControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "selectedItem", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SelectedItem = CreateForObjectType(valueToken, lastValueToken, context); });
                TrySetEvent(context, obj, lastObj, "SelectionChanged", t, (target, valueToken, lastValueToken) => SetSelectionChangedEventHandler(valueToken.ToString(), target));
            }
            static void SelectionChangedRouter(object sender, SelectionChangedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["SelectionChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetSelectionChangedEventHandler(string handlerName, Selector element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["SelectionChanged"] = handlerName;
                element.SelectionChanged -= SelectionChangedRouter;
                element.SelectionChanged += SelectionChangedRouter;
            }
        }

        internal static partial class ItemsControlHandler
        {
            internal static void SetProperties(ItemsControl t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "itemsSource", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { SetItemsSource(target, valueToken, lastValueToken, context); });
                TrySet(context, obj, lastObj,
                    "itemContainerTransitions", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { SetItemContainerTransitions(target, valueToken, lastValueToken, context); });
            }
        }

        internal static partial class RangeBaseHandler
        {
            internal static void SetProperties(RangeBase t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "minimum", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Minimum = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "maximum", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Maximum = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "value", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Value = valueToken.Value<double>());
                TrySetEvent(context, obj, lastObj, "ValueChanged", t, (target, valueToken, lastValueToken) => SetValueChangedEventHandler(valueToken.ToString(), target));
            }
            static void ValueChangedRouter(object sender, RangeBaseValueChangedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["ValueChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetValueChangedEventHandler(string handlerName, RangeBase element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["ValueChanged"] = handlerName;
                element.ValueChanged -= ValueChangedRouter;
                element.ValueChanged += ValueChangedRouter;
            }
        }

        // UNDONE: Content property (and others) now will recreate when child props change instead of incremental
        // update now that we drop "lastNamedObjectMap" on the floor and track references... 
        // 
        internal static partial class ButtonHandler
        {
            internal static Button Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Button>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(Button t, JObject obj, JObject lastObj, DiffContext context)
            {
                ButtonBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static partial class CalendarDatePickerHandler
        {
            internal static CalendarDatePicker Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<CalendarDatePicker>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(CalendarDatePicker t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "date", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { 
                    var curValue = target.Date; 
                    var newValue = DateTimeOffset.Parse(valueToken.ToString()); 
                    if (curValue != newValue) { target.Date = newValue; } 
                });
                TrySetEvent(context, obj, lastObj, "CalendarViewDayItemChanging", t, (target, valueToken, lastValueToken) => SetCalendarViewDayItemChangingEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "Closed", t, (target, valueToken, lastValueToken) => SetClosedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "DateChanged", t, (target, valueToken, lastValueToken) => SetDateChangedEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "Opened", t, (target, valueToken, lastValueToken) => SetOpenedEventHandler(valueToken.ToString(), target));
            }
            static void CalendarViewDayItemChangingRouter(object sender, CalendarViewDayItemChangingEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["CalendarViewDayItemChanging"], Sender = sender, EventArgs = e });
                }
            }
            static void SetCalendarViewDayItemChangingEventHandler(string handlerName, CalendarDatePicker element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["CalendarViewDayItemChanging"] = handlerName;
                element.CalendarViewDayItemChanging -= CalendarViewDayItemChangingRouter;
                element.CalendarViewDayItemChanging += CalendarViewDayItemChangingRouter;
            }
            static void ClosedRouter(object sender, Object e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["Closed"], Sender = sender, EventArgs = e });
                }
            }
            static void SetClosedEventHandler(string handlerName, CalendarDatePicker element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["Closed"] = handlerName;
                element.Closed -= ClosedRouter;
                element.Closed += ClosedRouter;
            }
            static void DateChangedRouter(object sender, CalendarDatePickerDateChangedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["DateChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetDateChangedEventHandler(string handlerName, CalendarDatePicker element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["DateChanged"] = handlerName;
                element.DateChanged -= DateChangedRouter;
                element.DateChanged += DateChangedRouter;
            }
            static void OpenedRouter(object sender, Object e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["Opened"], Sender = sender, EventArgs = e });
                }
            }
            static void SetOpenedEventHandler(string handlerName, CalendarDatePicker element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["Opened"] = handlerName;
                element.Opened -= OpenedRouter;
                element.Opened += OpenedRouter;
            }
        }
        internal static partial class CalendarViewHandler
        {
            internal static CalendarView Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<CalendarView>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(CalendarView t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "minDate", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { 
                    var curValue = target.MinDate; 
                    var newValue = DateTimeOffset.Parse(valueToken.ToString()); 
                    if (curValue != newValue) { target.MinDate = newValue; } 
                });
                TrySet(context, obj, lastObj,
                    "maxDate", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { 
                    var curValue = target.MaxDate; 
                    var newValue = DateTimeOffset.Parse(valueToken.ToString()); 
                    if (curValue != newValue) { target.MaxDate = newValue; } 
                });
                TrySetEvent(context, obj, lastObj, "CalendarViewDayItemChanging", t, (target, valueToken, lastValueToken) => SetCalendarViewDayItemChangingEventHandler(valueToken.ToString(), target));
                TrySetEvent(context, obj, lastObj, "SelectedDatesChanged", t, (target, valueToken, lastValueToken) => SetSelectedDatesChangedEventHandler(valueToken.ToString(), target));
            }
            static void CalendarViewDayItemChangingRouter(object sender, CalendarViewDayItemChangingEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["CalendarViewDayItemChanging"], Sender = sender, EventArgs = e });
                }
            }
            static void SetCalendarViewDayItemChangingEventHandler(string handlerName, CalendarView element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["CalendarViewDayItemChanging"] = handlerName;
                element.CalendarViewDayItemChanging -= CalendarViewDayItemChangingRouter;
                element.CalendarViewDayItemChanging += CalendarViewDayItemChangingRouter;
            }
            static void SelectedDatesChangedRouter(object sender, CalendarViewSelectedDatesChangedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["SelectedDatesChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetSelectedDatesChangedEventHandler(string handlerName, CalendarView element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["SelectedDatesChanged"] = handlerName;
                element.SelectedDatesChanged -= SelectedDatesChangedRouter;
                element.SelectedDatesChanged += SelectedDatesChangedRouter;
            }
        }
        internal static partial class RelativePanelHandler
        {
            internal static RelativePanel Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<RelativePanel>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(RelativePanel t, JObject obj, JObject lastObj, DiffContext context)
            {
                PanelHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static partial class RepositionThemeTransitionHandler
        {
            internal static RepositionThemeTransition Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<RepositionThemeTransition>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(RepositionThemeTransition t, JObject obj, JObject lastObj, DiffContext context)
            {
            }
        }
        internal static partial class ProgressBarHandler
        {
            internal static ProgressBar Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ProgressBar>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(ProgressBar t, JObject obj, JObject lastObj, DiffContext context)
            {
                RangeBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static partial class SliderHandler
        {
            internal static Slider Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Slider>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(Slider t, JObject obj, JObject lastObj, DiffContext context)
            {
                RangeBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        

        internal static partial class CheckBoxHandler
        {
            internal static CheckBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<CheckBox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(CheckBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                ButtonBaseHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "isChecked", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsChecked = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySetEvent(context, obj, lastObj, "Checked", t, (target, valueToken, lastValueToken) => SetCheckedEventHandler(valueToken.ToString(), target));
            }
            static void CheckedRouter(object sender, RoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["Checked"], Sender = sender, EventArgs = e });
                }
            }
            static void SetCheckedEventHandler(string handlerName, CheckBox element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["Checked"] = handlerName;
                element.Checked -= CheckedRouter;
                element.Checked += CheckedRouter;
            }
        }

        internal static partial class ScrollViewerHandler
        {
            internal static ScrollViewer Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ScrollViewer>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(ScrollViewer t, JObject obj, JObject lastObj, DiffContext context)
            {
                ContentControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "horizontalScrollBarVisibility", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalScrollBarVisibility = XamlStringParse<ScrollBarVisibility>(valueToken));
                TrySet(context, obj, lastObj,
                    "horizontalScrollMode", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalScrollMode = XamlStringParse<ScrollMode>(valueToken));
                TrySet(context, obj, lastObj,
                    "horizontalSnapPointsAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalSnapPointsAlignment = XamlStringParse<SnapPointsAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "horizontalSnapPointsType", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalSnapPointsType = XamlStringParse<SnapPointsType>(valueToken));
                TrySet(context, obj, lastObj,
                    "isDeferredScrollingEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsDeferredScrollingEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isHorizontalRailEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsHorizontalRailEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isHorizontalScrollChainingEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsHorizontalScrollChainingEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isScrollInertiaEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsScrollInertiaEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isVerticalRailEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsVerticalRailEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isVerticalScrollChainingEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsVerticalScrollChainingEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isZoomChainingEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsZoomChainingEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isZoomInertiaEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsZoomInertiaEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "maxZoomFactor", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.MaxZoomFactor = valueToken.Value<float>());
                TrySet(context, obj, lastObj,
                    "minZoomFactor", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.MinZoomFactor = valueToken.Value<float>());
                TrySet(context, obj, lastObj,
                    "leftHeader", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.LeftHeader = CreateFromState(valueToken, lastValueToken, context) as UIElement);
                TrySet(context, obj, lastObj,
                    "topHeader", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TopHeader = CreateFromState(valueToken, lastValueToken, context) as UIElement);
                TrySet(context, obj, lastObj,
                    "topLeftHeader", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TopLeftHeader = CreateFromState(valueToken, lastValueToken, context) as UIElement);
                TrySet(context, obj, lastObj,
                    "verticalScrollBarVisibility", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalScrollBarVisibility = XamlStringParse<ScrollBarVisibility>(valueToken));
                TrySet(context, obj, lastObj,
                    "verticalScrollMode", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalScrollMode = XamlStringParse<ScrollMode>(valueToken));
                TrySet(context, obj, lastObj,
                    "verticalSnapPointsAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalSnapPointsAlignment = XamlStringParse<SnapPointsAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "verticalSnapPointsType", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalSnapPointsType = XamlStringParse<SnapPointsType>(valueToken));
                TrySet(context, obj, lastObj,
                    "zoomMode", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.ZoomMode = XamlStringParse<ZoomMode>(valueToken));
                TrySet(context, obj, lastObj,
                    "zoomSnapPointsType", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.ZoomSnapPointsType = XamlStringParse<SnapPointsType>(valueToken));
            }
        }

        internal static partial class ButtonBaseHandler
        {
            internal static void SetProperties(ButtonBase t, JObject obj, JObject lastObj, DiffContext context)
            {
                ContentControlHandler.SetProperties(t, obj, lastObj, context);
                TrySetEvent(context, obj, lastObj, "Click", t, (target, valueToken, lastValueToken) => SetClickEventHandler(valueToken.ToString(), target));
            }
            static void ClickRouter(object sender, RoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["Click"], Sender = sender, EventArgs = e });
                }
            }
            static void SetClickEventHandler(string handlerName, ButtonBase element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["Click"] = handlerName;
                element.Click -= ClickRouter;
                element.Click += ClickRouter;
            }
        }
        
        internal static partial class ContentControlHandler
        {
            internal static ContentControl Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ContentControl>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(ContentControl t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "content", 
                    true,
                    t,
                    (target, valueToken, lastValueToken) => target.Content = CreateFromState(valueToken, lastValueToken, context));
            }
        }

        internal static partial class ControlHandler
        {
            internal static void SetProperties(Control t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "background", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Background = XamlStringParse<Brush>(valueToken));
                TrySet(context, obj, lastObj,
                    "foreground", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Foreground = XamlStringParse<Brush>(valueToken));
                TrySet(context, obj, lastObj,
                    "fontFamily", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontFamily = new FontFamily(valueToken.ToString()));
                TrySet(context, obj, lastObj,
                    "fontSize", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontSize = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "fontWeight", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontWeight = ParseEnum<FontWeight>(valueToken));
            }
        }

        internal static partial class StackPanelHandler
        {
            internal static StackPanel Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<StackPanel>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(StackPanel t, JObject obj, JObject lastObj, DiffContext context)
            {
                PanelHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "orientation", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Orientation = ParseEnum<Orientation>(valueToken));
            }
        }

        internal static partial class GridHandler
        {
            internal static Grid Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Grid>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(Grid t, JObject obj, JObject lastObj, DiffContext context)
            {
                PanelHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "rows", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { PanelHandler.SetGridRowDefinitions(target, (JArray)valueToken); });
                TrySet(context, obj, lastObj,
                    "columns", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { PanelHandler.SetGridColumnDefinitions(target, (JArray)valueToken); });
            }
        }

        internal static partial class MapControlHandler 
        {
            static void SetChildren(MapControl t, JObject obj, JObject lastObj, DiffContext context)
            {
                List<DependencyObject> children = new List<DependencyObject>();
                IJEnumerable<JToken> lastChildren = null;
                JToken last;
                if (lastObj != null && lastObj.TryGetValue("children", out last))
                {
                    lastChildren = last.AsJEnumerable();
                }
                CollectChildrenWorker(obj["children"].AsJEnumerable(), lastChildren, children, context);
                var setChildrenNeeded = false;
                if (t.Children.Count == children.Count)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (!object.ReferenceEquals(children[i], t.Children[i]))
                        {
                            setChildrenNeeded = true;
                        }
                    }
                }
                else
                {
                    setChildrenNeeded = true;
                }

                if (setChildrenNeeded)
                {
                    t.Children.Clear();
                    foreach (var child in children)
                    {
                        t.Children.Add(child);
                    }
                }
            }

            static void SetMapElements(MapControl t, JObject obj, JObject lastObj, DiffContext context)
            {
                List<DependencyObject> children = new List<DependencyObject>();
                IJEnumerable<JToken> lastChildren = null;
                JToken last;
                if (lastObj != null && lastObj.TryGetValue("mapElements", out last))
                {
                    lastChildren = last.AsJEnumerable();
                }
                CollectChildrenWorker(obj["mapElements"].AsJEnumerable(), lastChildren, children, context);
                var setChildrenNeeded = false;
                if (t.Children.Count == children.Count)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (!object.ReferenceEquals(children[i], t.Children[i]))
                        {
                            setChildrenNeeded = true;
                        }
                    }
                }
                else
                {
                    setChildrenNeeded = true;
                }

                if (setChildrenNeeded)
                {
                    t.MapElements.Clear();
                    foreach (var child in children.OfType<MapElement>())
                    {
                        t.MapElements.Add(child);
                    }
                }
            }
        }

        internal static class PanelHandler
        {
            internal static void SetGridRowDefinitions(Grid t, JArray obj) 
            {
                t.RowDefinitions.Clear();
                foreach (var d in obj.AsJEnumerable())
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = XamlStringParse<GridLength>(d);
                    t.RowDefinitions.Add(rd);
                }
            }
            internal static void SetGridColumnDefinitions(Grid t, JArray obj) 
            {
                t.ColumnDefinitions.Clear();
                foreach (var d in obj.AsJEnumerable())
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.Width = XamlStringParse<GridLength>(d);
                    t.ColumnDefinitions.Add(cd);
                }
            }

            static void SetPanelChildren(Panel t, JObject obj, JObject lastObj, DiffContext context)
            {
                List<UIElement> children = new List<UIElement>();
                IJEnumerable<JToken> lastChildren = null;
                JToken last;
                if (lastObj != null && lastObj.TryGetValue("children", out last))
                {
                    lastChildren = last.AsJEnumerable();
                }
                CollectChildrenWorker(obj["children"].AsJEnumerable(), lastChildren, children, context);
                var setChildrenNeeded = false;
                if (t.Children.Count == children.Count)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (!object.ReferenceEquals(children[i], t.Children[i]))
                        {
                            setChildrenNeeded = true;
                        }
                    }
                }
                else
                {
                    setChildrenNeeded = true;
                }

                if (setChildrenNeeded)
                {
                    t.Children.Clear();
                    foreach (var child in children)
                    {
                        Unparent(child);
                        t.Children.Add(child);
                    }
                }
            }
            internal static void SetProperties(Panel t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                SetPanelChildren(t, obj, lastObj, context);
                TrySet(context, obj, lastObj, "background", false, t, (target, valueToken, lastValueToken) => target.Background = XamlStringParse<Brush>(valueToken));
                TrySet(context, obj, lastObj, "childrenTransitions", false, t, (target, valueToken, lastValueToken) => { SetChildrenTransitions(target, valueToken, lastValueToken, context); });
            }

        }

        static void CollectChildrenWorker<T>(IJEnumerable<JToken> items, IEnumerable<JToken> lastItems, List<T> children, DiffContext context) where T:DependencyObject
        {
            IEnumerator<JToken> enumerator = null;
            if (lastItems != null)
            {
                enumerator = lastItems.GetEnumerator();
                enumerator.Reset();
            }
            if(items != null)
            {
                foreach (var child in items)
                {
                    JToken lastChild = null;
                    if (enumerator != null && enumerator.MoveNext()) { lastChild = enumerator.Current; }

                    if (child.Type == JTokenType.Array)
                    {
                        CollectChildrenWorker(child.AsJEnumerable(), lastChild != null ? lastChild.AsJEnumerable() : null, children, context);
                    }
                    else
                    {
                        var instance = CreateFromState((JObject)child, lastChild as JObject, context);
                        children.Add((T)instance);
                    }
                }
            }
        }

        static DependencyProperty eventMap = DependencyProperty.RegisterAttached("XSEventMap", typeof(Dictionary<string, string>), typeof(FrameworkElement), PropertyMetadata.Create((object)null));
        static Dictionary<string, CreateCallback> handlers;

        public static event EventHandler<CommandEventArgs> Command;


        delegate void SetCollectionPropertyCallback<TObject, TValue>(TObject target, List<TValue> items);
        internal static void SetItemContainerTransitions(ItemsControl control, JToken obj, JToken last, Handler.DiffContext context)
        {
            SetCollectionProperty<ItemsControl, Transition>(
                control,
                "itemContainerTransitions",
                obj,
                last,
                context,
                (target, list) =>
                {
                    var col = target.ItemContainerTransitions;
                    if (col == null)
                    {
                        target.ItemContainerTransitions = col = new TransitionCollection();
                    }
                    else
                    {
                        col.Clear();
                    }
                    if (list != null)
                    {
                        foreach (var child in list)
                        {
                            col.Add(child);
                        }
                    }
                });

        }
        internal static void SetChildrenTransitions(Panel control, JToken obj, JToken last, Handler.DiffContext context)
        {
            SetCollectionProperty<Panel, Transition>(
                control,
                "itemContainerTransitions",
                obj,
                last,
                context,
                (target, list) =>
                {
                    TransitionCollection col = target.ChildrenTransitions;
                    if (col == null)
                    {
                        target.ChildrenTransitions = col = new TransitionCollection();
                    }
                    else
                    {
                        col.Clear();
                    }
                    if (list != null)
                    {
                        foreach (var child in list)
                        {
                            col.Add(child);
                        }
                    }
                });

        }

        static void SetCollectionProperty<TObject, TValue>(
            TObject t, 
            string propertyName,
            JToken obj,
            JToken lastObj,
            Handler.DiffContext context, 
            SetCollectionPropertyCallback<TObject, TValue> setter) where TValue : DependencyObject
        {
            List<TValue> children = new List<TValue>();
            IJEnumerable<JToken> lastChildren = null;
            if (lastObj != null)
            {
                lastChildren = lastObj.AsJEnumerable();
            }
            CollectItemsWorker(t, obj.AsJEnumerable(), lastChildren, children, context);
            // UNDONE: better diff
            //
            var setChildrenNeeded = true;

            if (setChildrenNeeded)
            {
                setter(t, children);
            }
        }
        static void CollectItemsWorker<TObject, TValue>(
            TObject t, 
            IJEnumerable<JToken> items, 
            IEnumerable<JToken> lastItems, 
            List<TValue> children,
            Handler.DiffContext context) where TValue : DependencyObject
        {
            IEnumerator<JToken> enumerator = null;
            if (lastItems != null)
            {
                enumerator = lastItems.GetEnumerator();
                enumerator.Reset();
            }
            foreach (var child in items)
            {
                JToken lastChild = null;
                if (enumerator != null && enumerator.MoveNext()) { lastChild = enumerator.Current; }

                if (child.Type == JTokenType.Array)
                {
                    CollectItemsWorker(t, child.AsJEnumerable(), lastChild != null ? lastChild.AsJEnumerable() : null, children, context);
                }
                else
                {
                    var instance = Handler.CreateFromState((JObject)child, lastChild as JObject, context);
                    children.Add((TValue)instance);
                }
            }
        }

        internal static object CreateForObjectType(JToken obj, JToken objLast, Handler.DiffContext context)
        {
            switch (obj.Type)
            {
                case JTokenType.Float:
                    return obj.Value<double>();
                case JTokenType.Integer:
                    return obj.Value<int>();
                case JTokenType.String:
                    return obj.Value<string>();
                case JTokenType.Object:
                    var instance = Handler.CreateFromState((JObject)obj, objLast as JObject, context);
                    return instance;
                default:
                    return "Unhandled:" + Enum.GetName(typeof(JTokenType), obj.Type);
            }
        }

        static void AddListViewFixups(ListView lv, JObject obj, JObject lastObj, DiffContext context)
        {
#if USE_LISTVIEW_VIRTUALIZATION
            // UNDONE: ShowsScrollingPlaceholder for even more hotness

            lv.ChoosingItemContainer += delegate (ListViewBase sender, ChoosingItemContainerEventArgs args)
            {
                args.IsContainerPrepared = true;

                ListViewItem itemContainer = args.ItemContainer as ListViewItem;
                if (itemContainer == null) { itemContainer = new ListViewItem(); }
                args.ItemContainer = itemContainer;
            };
            lv.ContainerContentChanging += delegate (ListViewBase sender, ContainerContentChangingEventArgs args)
            {
                args.Handled = true;

                ListViewItem itemContainer = args.ItemContainer as ListViewItem;
                ItemEntry ie = args.Item as ItemEntry;
                if (ie != null)
                {
                    var rendered = CreateForObjectType(ie.child, ie.comp, context) as UIElement;
                    if (rendered != itemContainer.Content)
                    {
                        Unparent(rendered);
                        itemContainer.Content = rendered;
                    }
                }
                else
                {
                    itemContainer.Content = args.Item;
                }
            };
#endif
        }

#if USE_LISTVIEW_VIRTUALIZATION
        class ItemEntry {
            public JToken child;
            public JToken comp;
        }
#endif

        static List<object> CollectItemsSourceItems(JToken source, JToken lastSource, Handler.DiffContext context)
        {
            // UNDONE: need to do better delta on previous version of the list
            //
            List<object> collection = new List<object>();
            if (source.Type == JTokenType.Array)
            {
                IJEnumerable<JToken> lastSourceEnumerable = null;
                IEnumerator<JToken> lastSourceEnum = null;
                if (lastSource != null && lastSource.Type == JTokenType.Array)
                {
                    lastSourceEnumerable = lastSource.AsJEnumerable();
                    lastSourceEnum = lastSourceEnumerable.GetEnumerator();
                    lastSourceEnum.MoveNext();
                }

                foreach (var child in source.AsJEnumerable())
                {
                    JToken comp = null;
                    if (lastSourceEnum != null)
                    {
                        comp = lastSourceEnum.Current;
                        lastSourceEnum.MoveNext();
                    }
#if USE_LISTVIEW_VIRTUALIZATION
                    collection.Add(new ItemEntry() { child=child, comp=comp });
#else
                    collection.Add(CreateForObjectType(child, comp, context));
#endif
                }
            }
            return collection;
        }

        internal static void SetItems(MapItemsControl control, JToken source, JToken lastSource, Handler.DiffContext context)
        {
            // UNDONE: can't diff... for some reason :(
            context.BeginForceCreate();
            var items = CollectItemsSourceItems(source, lastSource, context);
            context.EndForceCreate();
            // var items = CollectItemsSourceItems(source, lastSource, context);
            // foreach (var child in control.Items) 
            // {
            //     Unparent((UIElement)child);
            // }
            control.Items.Clear();
            foreach (var i in items) 
            {
                control.Items.Add((DependencyObject)i);
            }
        }

        internal static void SetItemsSource(ItemsControl control, JToken source, JToken lastSource, Handler.DiffContext context)
        {
            control.ItemsSource = CollectItemsSourceItems(source, lastSource, context);
        }

        static CreateResult<T> CreateOrGetLast<T>(JObject obj, DiffContext context) where T:new()
        {
            JToken name;
            string resolvedName = null;
            if (obj.TryGetValue("name", out name))
            {
                resolvedName = name.ToString();
            }
            else
            {
                resolvedName = context.CreateSurrogateName(obj);
            }

            if (resolvedName != null)
            { 
                object value;
                if (context.TryGetObject(resolvedName, out value))
                {
                    if (value != null && value is T)
                    {
                        return new CreateResult<T>() { Value = (T)value, Recycled = true, Name = resolvedName };
                    }
                }
            }
            context.ObjectCreateCount++;
            var instance = new T();
            context.AddObject(resolvedName, instance);
            return new CreateResult<T>() { Value = instance, Recycled = false, Name = resolvedName };
        }
        static void Unparent(UIElement child)
        {
            DependencyObject visualParent = null;

            var fe = child as FrameworkElement;
            if (fe != null && fe.Parent != null)
            {
                visualParent = fe.Parent;
            }
            if (visualParent == null)
            {
                visualParent = VisualTreeHelper.GetParent(child);
            }
            var parentContent = visualParent as ContentControl;
            var parentPresenter = visualParent as ContentPresenter;
            var parentBorder = visualParent as Border;
            var parentPanel = visualParent as Panel;
            if (parentPanel != null)
            {
                parentPanel.Children.Remove(child);
            }
            else if (parentContent != null)
            {
                parentContent.Content = null;
            }
            else if (parentBorder != null)
            {
                parentBorder.Child = null;
            }
            else if (parentPresenter != null)
            {
                parentPresenter.Content = null;
            }
        }
        static void TrySet<T>(DiffContext context, JObject obj, JObject last, string name, T target, Setter<T> setter)
        {
            TrySet<T>(context, obj, last, name, false, target, setter);
        }
        static void TrySet<T>(DiffContext context, JObject obj, JObject last, string name, bool aliasFirstChild, T target, Setter<T> setter)
        {
            TrySet<T>(context, obj, last, name, aliasFirstChild, target, setter, null);
        }
        static void TrySet<T>(DiffContext context, JObject obj, JObject last, string name, bool aliasFirstChild, T target, Setter<T> setter, List<DeferSetter> defer)
        {
            JToken tok;
            JToken tokLast = null;
            bool found = false;
            if (!obj.TryGetValue(name, out tok))
            {
                if (aliasFirstChild && obj.TryGetValue("children", out tok))
                {
                    found = true;
                    tok = ((JArray)tok).First;
                }
            }
            else
            {
                found = true;
            }
            if (found)
            {
                if (last != null)
                {
                    if (!last.TryGetValue(name, out tokLast))
                    {
                        if (aliasFirstChild && last.TryGetValue("children", out tokLast))
                        {
                            tokLast = ((JArray)tokLast).First;
                        }
                    }

                    if (tokLast != null && tokLast.ToString() == tok.ToString())
                    {
                        return; // bail early if old & new are the same
                    }
                }
                context.PropertySetCount++;
                if (defer != null) 
                {
                    defer.Add(new DeferSetter<T>() { setter = setter, target = target, tok = tok, tokLast = tokLast });
                }
                else 
                {
                    setter(target, tok, tokLast);
                }
            }
        }
        static void TrySetEvent<T>(DiffContext context, JObject obj, JObject last, string name, T target, Setter<T> setter)
        {
            JToken tok;
            JToken tokLast = null;
            if (obj.TryGetValue("on" + name, out tok))
            {
                if (last != null && last.TryGetValue("on" + name, out tokLast))
                {
                    if (tokLast.ToString() == tok.ToString())
                    {
                        return; // bail early if old & new are the same
                    }
                }
                context.EventSetCount++;
                setter(target, tok, tokLast);
            }
        }
        static T ParseEnum<T>(JToken v) 
        {
            return (T)Enum.Parse(typeof(T), v.ToString());
        }
        static T XamlStringParse<T>(JToken v)
        {
            return (T)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(T), v.ToString());
        }
        static Dictionary<string, CreateCallback> GetHandlers()
        {
            if (handlers == null)
            {
                handlers = new Dictionary<string, CreateCallback>();
                handlers["MapItemsControl"] = MapItemsControlHandler.Create;
                handlers["MapPolygon"] = MapPolygonHandler.Create;
                handlers["MapControl"] = MapControlHandler.Create;
                handlers["Ellipse"] = EllipseHandler.Create;
                handlers["Rectangle"] = RectangleHandler.Create;
                handlers["Viewbox"] = ViewboxHandler.Create;
                handlers["TextBlock"] = TextBlockHandler.Create;
                handlers["Image"] = ImageHandler.Create;
                handlers["RichEditBox"] = RichEditBoxHandler.Create;
                handlers["TextBox"] = TextBoxHandler.Create;
                handlers["GridView"] = GridViewHandler.Create;
                handlers["ListView"] = ListViewHandler.Create;
                handlers["ComboBox"] = ComboBoxHandler.Create;
                handlers["ListBox"] = ListBoxHandler.Create;
                handlers["Button"] = ButtonHandler.Create;
                handlers["CalendarDatePicker"] = CalendarDatePickerHandler.Create;
                handlers["CalendarView"] = CalendarViewHandler.Create;
                handlers["RelativePanel"] = RelativePanelHandler.Create;
                handlers["RepositionThemeTransition"] = RepositionThemeTransitionHandler.Create;
                handlers["ProgressBar"] = ProgressBarHandler.Create;
                handlers["Slider"] = SliderHandler.Create;
                handlers["CheckBox"] = CheckBoxHandler.Create;
                handlers["ScrollViewer"] = ScrollViewerHandler.Create;
                handlers["ContentControl"] = ContentControlHandler.Create;
                handlers["StackPanel"] = StackPanelHandler.Create;
                handlers["Grid"] = GridHandler.Create;
            }
            return handlers;
        }
        internal static DependencyObject CreateFromState(JToken item, JToken lastItem, DiffContext context)
        {
            if (item.Type == JTokenType.Object)
            {
                var type = item["type"].ToString();
                CreateCallback create;
                if (GetHandlers().TryGetValue(type, out create))
                {
                    return create((JObject)item, (JObject)lastItem, context);
                }
                return new TextBlock() { FontSize = 48, Text = "'" + type + "'Not found" };
            }
            else
            {
                return new TextBlock() { Text = item.ToString() };
            }
        }
        internal static Uri FromRelativeUri(FrameworkElement relativeElement, string path, DiffContext context)
        {
            var r = relativeElement;
            var b = r.BaseUri;
            while (b == null)
            {
                r = VisualTreeHelper.GetParent(r) as FrameworkElement;
                if (r == null) { b = context.HostElement.BaseUri; break; }
                b = r.BaseUri;
            }
            if (b == null) { throw new InvalidOperationException("Can't determine base uri"); }
            var u = new Uri(b, path);
            return u;
        }

        internal delegate void Setter<T>(T target, JToken value, JToken lastValue);
        delegate DependencyObject CreateCallback(JObject obj, JObject lastObj, DiffContext context);

        struct CreateResult<T> 
        {
            public T Value;
            public bool Recycled;
            public string Name;
        }
        internal abstract class DeferSetter
        {
            public abstract void Do();
        }
        internal class DeferSetter<T> : DeferSetter
        {
            internal T target;
            internal JToken tok;
            internal JToken tokLast;
            internal Setter<T> setter;

            public override void Do()
            {
                setter(target, tok, tokLast);
            }
        }
        internal class DiffContext 
        {
            Dictionary<string, object> lastNamedObjectMap;
            Dictionary<string, object> currentNamedObjectMap = new Dictionary<string, object>();
            Dictionary<string, int> surrogateKeys = new Dictionary<string, int>();
            List<string> nameStack = new List<string>();
            int forceCreateCount = 0;

            internal DiffContext(Dictionary<string, object> lastNamedObjectMap)
            {
                this.lastNamedObjectMap = lastNamedObjectMap;
            }

            public List<DeferSetter> Defer;
            public int EventSetCount;
            public int PropertySetCount;
            public int ObjectCreateCount;
            public DateTime Start;
            public DateTime End;
            public FrameworkElement HostElement;
                
            public void BeginForceCreate() 
            {
                forceCreateCount++;
            }
            public void EndForceCreate()
            {
                forceCreateCount--;
            }
            public Dictionary<String, object> GetNamedObjectMap() { return currentNamedObjectMap; }

            public object ReferenceObject(string name) 
            {
                object value;
                if (lastNamedObjectMap.TryGetValue(name, out value))
                {
                    currentNamedObjectMap[name] = value;
                    return value;
                }
                return null;
            }
            public bool TryGetObject(string name, out object value)
            {
                if (forceCreateCount > 0) { value = null; return false; }

                var ret = lastNamedObjectMap.TryGetValue(name, out value);
                if (ret) { currentNamedObjectMap[name] = value; }
                return ret;
            }
            public void AddObject(string name, object value)
            {
                lastNamedObjectMap[name] = currentNamedObjectMap[name] = value;
            }
            public string CreateSurrogateName(JObject obj) 
            { 
                string baseName = "xsrt";
                JToken t;
                if (obj.TryGetValue("type", out t))
                {
                    baseName = t.ToString();
                }
                int n = 0;
                if (surrogateKeys.TryGetValue(baseName, out n)) 
                {
                    n++;
                }
                surrogateKeys[baseName] = n;
                string key = baseName + n;

                var concat = nameStack.Count > 0 ? nameStack.Aggregate((a, b) => a + "-" + b) : "root";
                 
                return concat + "-" + key;
            }
            public void PushName(string name)
            {
                nameStack.Add(name);
            }
            public void PopName(string name)
            {
                var n = nameStack[nameStack.Count - 1];
                if (name != n) { throw new InvalidOperationException("unbalanced name"); }
                nameStack.RemoveAt(nameStack.Count - 1);
            }
        }
    }
}

