declare var host: any;
declare var xsrt: any;

module React {
    declare var App: any;

    function applyMembers(result, members) {
        if (members) {
            var keys = Object.keys(members)
            for (var i = 0; i < keys.length; i++) {
                var key = keys[i];
                var value = members[key];
                if (value instanceof Function) {
                    var funName = "$" + (funcCount++);
                    App.eventHandlers = App.eventHandlers || {};
                    App.eventHandlers[funName] = value;
                    result[key] = funName;
                }
                else if (key == "style") {
                    applyMembers(result, value);
                }
                else {
                    result[key] = members[key];
                }
            }
        }
    }

    var funcCount = 1;
    export function createElement(ctor, members) {
        var result = new ctor();
        applyMembers(result, members);
        if (arguments.length > 2) {
            result.children = Array.prototype.slice.call(arguments, 2);
        }
        return result;
    }

    export function __spread(target, record1, record2) {
        var o : any = Object;
        return o.assign(target, record1, record2);
        
    }
}

module JSX {
    export interface ElementAttributesProperty {
       props: any;
    }
}

module Xaml {
    export interface Geopoint {
        latitude: number;
        longitude: number;
    }
    export interface MapItemsControlProps {
        name?: string; // UNDONE: move to more "root" object for sharing
        items?: any; // UNDONE
    }
    export interface MapControlProps extends ControlProps {
        // UNDONE: ActualCamera
        businessLandmarksVisible?: boolean;
        center?: Geopoint;
        // UNDONE: Children
        // UNDONE: public MapColorScheme ColorScheme { get; set; }
        // UNDONE: public MapCustomExperience CustomExperience { get; set; }
        desiredPitch?: number;
        heading?: number;
        // READONLY: BooleanProp("Is3DSupported"),
        // READONLY: BooleanProp("IsStreetsideSupported"),
        landmarksVisible?: boolean;
        // UNDONE: public MapLoadingStatus LoadingStatus { get; }
        // UNDONE: public IList<MapElement> MapElements { get; }
        mapServiceToken: string; // not optional
        // READONLY: DoubleProp("MaxZoomLevel"),
        // READONLY: DoubleProp("MinZoomLevel"),
        // UNDONE: public MapPanInteractionMode PanInteractionMode { get; set; }
        pedestrianFeaturesVisible?: boolean;
        // READONLY: DoubleProp("Pitch"),
        // UNDONE: public MapInteractionMode RotateInteractionMode { get; set; }
        // UNDONE: public IList<MapRouteView> Routes { get; }
        // UNDONE: public MapScene Scene { get; set; }
        // UNDONE: public MapStyle Style { get; set; }
        // UNDONE: public MapCamera TargetCamera { get; }
        // UNDONE: public IList<MapTileSource> TileSources { get; }
        // UNDONE: public MapInteractionMode TiltInteractionMode { get; set; }
        trafficFlowVisible?: boolean;
        // UNDONE: public Point TransformOrigin { get; set; }
        transitFeaturesVisible?: boolean;
        // UNDONE: public MapWatermarkMode WatermarkMode { get; set; }
        // UNDONE: public MapInteractionMode ZoomInteractionMode { get; set; }
        zoomLevel?: number;

        onActualCameraChanged?: any; // callback? 
        onActualCameraChanging?: any; // callback? 
        onCenterChanged?: any; // callback? 
        onCustomExperienceChanged?: any; // callback? 
        onHeadingChanged?: any; // callback? 
        onLoadingStatusChanged?: any; // callback? 
        onMapDoubleTapped?: any; // callback? 
        onMapElementClick?: any; // callback? 
        onMapElementPointerEntered?: any; // callback? 
        onMapElementPointerExited?: any; // callback? 
        onMapHolding?: any; // callback? 
        onMapTapped?: any; // callback? 
        onPitchChanged?: any; // callback? 
        onTargetCameraChanged?: any; // callback? 
        onTransformOriginChanged?: any; // callback? 
        onZoomLevelChanged?: any; // callback? 

    }
    export interface ShapeProps extends FrameworkElementProps {
        fill?: string;
        stroke?: string;
        strokeThickness?: number;
    }
    export interface EllipseProps extends ShapeProps {

    }
    export interface RectangleProps extends ShapeProps {

    }

    export interface MapControlAttachedProps {
        map$location?: Geopoint
    }
    export interface ScrollViewerAttachedProps {
        scrollViewer$verticalScrollBarVisibility?: string;
        scrollViewer$horizontalScrollBarVisibility?: string;
    }
    export interface GridAttachedProps {
        grid$row?: number;
        grid$rowSpan?: number;
        grid$column?: number;
        grid$columnSpan?: number;
    }
    export interface AccessibilityProps {
        automationId?: string;
        acc$helpText?: string;
        acc$labeledBy?: string; // ref?
        acc$liveSetting?: string;
        acc$name?: string;
    }
    export interface RelativePanelAttachedProps {
        relative$above?: string; //ref?
        relative$alignBottomWith?: string; //ref?
        relative$alignBottomWithPanel?: boolean;
        relative$alignHorizontalCenterWith?: string; //ref?
        relative$alignLeftWith?: string; //ref?
        relative$alignLeftWithPanel?: boolean;
        relative$alignRightWith?: string; //ref?
        relative$alignRightWithPanel?: boolean;
        relative$alignTopWith?: string; //ref?
        relative$alignTopWithPanel?: boolean;
        relative$alignVerticalCenterWith?: string; //ref?
        relative$alignVerticalCenterWithPanel?: boolean;
        relative$below?: string; //ref?
        relative$leftOf?: string; //ref?
        relative$rightOf?: string; //ref?
    }

    export interface UIElementProps {
        style?: {}; // UNDONE
        onKeyDown?: any; // callback?
        onKeyUp?: any; // callback?
    }
    export interface FrameworkElementProps extends UIElementProps, ScrollViewerAttachedProps, GridAttachedProps, AccessibilityProps, RelativePanelAttachedProps, MapControlAttachedProps {
        name?: string;
        width?: number;
        height?: number;
        horizontalAlignment?: string;
        verticalAlignment?: string;
        margin?: string;
    }
    export interface ViewboxProps extends FrameworkElementProps {
        stretch?: string;
        stretchDirection?: string;
        child?: any;
    }
    export interface TextBlockProps extends FrameworkElementProps {
        foreground?: string;
        text?: string;
        fontFamily?: string;
        fontSize?: number;
        fontWeight?: string;
    }
    export interface ImageProps extends FrameworkElementProps {
        source?: string;
    }
    export interface TextBoxSharedProps extends ControlProps {
        acceptsReturn?: boolean;
        text?: string;
        isColorFontEnabled?: boolean;
        isReadOnly?: boolean;
        isSpellCheckEnabled?: boolean;
        isTextPredictionEnabled?: boolean;
        placeholderText?: string;
        preventKeyboardDisplayOnProgrammaticFocus?: boolean;
        textAlignment?: string;
        textReadingOrder?: string;
        textWrapping?: string;
        onTextChanged?: any; // callback?
    }
    export interface RichEditBoxProps extends TextBoxSharedProps {

    }
    export interface TextBoxProps extends TextBoxSharedProps {
        selectionLength?: number;
        selectionStart?: number;
    }
    export interface ItemsControlProps extends ControlProps {
        itemsSource?: any; // UNDONE
        itemContainerTransitions?: any;
    }
    export interface SelectorProps extends ItemsControlProps {
        selectedItem?: any;
        onSelectionChanged?: any; // callback?
    }
    export interface ListViewBaseProps extends SelectorProps {

    }
    export interface ListViewProps extends ListViewBaseProps {

    }
    export interface GridViewProps extends ListViewBaseProps {

    }
    export interface ComboBoxProps extends SelectorProps {

    }
    export interface ListBoxProps extends SelectorProps {

    }
    export interface RangeBaseProps extends ControlProps {
        minimum?: number;
        maximum?: number;
        value?: number;
        onValueChanged?: any; // callback?
    }
    export interface ButtonProps extends ButtonBaseProps {

    }
    export interface ButtonBaseProps extends ControlProps {

    }
    export interface CalendarDatePickerProps extends ControlProps {
        date?: string; // UNDONE
        onCalendarViewDayItemChanging?: any; // callback?
        onClosed?: any; // callback?
        onDateChanged?: any; // callback?
        onOpened?: any; // callback?
    }
    export interface CalendarViewProps extends ControlProps {
        minDate?: string; // UNDONE
        maxDate?: string; // UNDONE
        onCalendarViewDayItemChanging?: any; // callback?
        onSelectedDatesChanged?: any; // callback?
    }
    export interface RelativePanelProps extends PanelProps {

    }
    export interface RepositionThemeTransitionProps {

    }
    export interface ProgressBarProps extends RangeBaseProps {

    }
    export interface SliderProps extends RangeBaseProps {

    }
    export interface CheckBoxProps extends ButtonBaseProps {
        isChecked?: boolean;
        onChecked?: any; // callback?
    }
    export interface ScrollViewerProps extends ContentControlProps {
        horizontalScrollBarVisibility?: string;
        horizontalScrollMode?: string;
        horizontalSnapPointsAlignment?: string;
        horizontalSnapPointsType?: string;
        isDeferredScrollingEnabled?: boolean;
        isHorizontalRailEnabled?: boolean;
        isHorizontalScrollChainingEnabled?: boolean;
        isScrollInertiaEnabled?: boolean;
        isVerticalRailEnabled?: boolean;
        isVerticalScrollChainingEnabled?: boolean;
        isZoomChainingEnabled?: boolean;
        isZoomInertiaEnabled?: boolean;
        maxZoomFactor?: number;
        minZoomFactor?: number;
        leftHeader?: string; // ref?
        topHeader?: string; // ref?
        topLeftHeader?: string; // ref?
        verticalScrollBarVisibility?: string;
        verticalScrollMode?: string;
        verticalSnapPointsAlignment?: string;
        verticalSnapPointsType?: string;
        zoomMode?: string;
        // UNDONE: public IList System.Single ZoomSnapPoints { get; }
        zoomSnapPointsType?: string;

    }
    export interface ButtonBaseProps extends ContentControlProps {
        onClick?: any; // callback?
    }
    export interface ContentControlProps extends ControlProps {
        content?: any;
    }
    export interface ControlProps extends FrameworkElementProps {
        background?: string;
        foreground?: string;
        fontFamily?: string;
        fontSize?: number;
        fontWeight?: string;
    }
    export interface PanelProps extends FrameworkElementProps {
        background?: string;
        childrenTransitions?: any; 
        // UNDONE
    }
    export interface StackPanelProps extends PanelProps {
        orientation?: string;
    }
    export interface GridProps extends PanelProps {
        rows?: [string];
        columns?: [string];
    }
    
    export class MapItemsControl {
        type: string;
        constructor() {
            this.type = 'MapItemsControl';
        }
        props: MapItemsControlProps
    };

    export class MapControl {
        type: string;
        constructor() {
            this.type = 'MapControl';
        }
        props: MapControlProps
    };

    export class Ellipse { 
        type: string;
        constructor() {
            this.type = 'Ellipse';
        }
        props: EllipseProps
    };
    export class Rectangle { 
        type: string;
        constructor() {
            this.type = 'Rectangle';
        }
        props: RectangleProps
    };
    export class Viewbox { 
        type: string;
        constructor() {
            this.type = 'Viewbox';
        }
        props: ViewboxProps
    };
    export class ScrollViewer {
        type: string;
        constructor() {
            this.type = 'ScrollViewer';
        }
        props: ScrollViewerProps;
    };
    export class GridView {
        type: string;
        constructor() {
            this.type = 'GridView';
        }
        props: GridViewProps;
    };
    export class ListView {
        type: string;
        constructor() {
            this.type = 'ListView';
        }
        props: ListViewProps;
    };
    export class Grid {
        type: string;
        constructor() {
            this.type = 'Grid';
        }
        props: GridProps;
    };
    export class StackPanel {
        type: string;
        constructor() {
            this.type = 'StackPanel';
        }
        props: StackPanelProps;
    };
    export class Image {
        type: string;
        constructor() {
            this.type = 'Image';
        }
        props: ImageProps;
    };
    export class TextBlock {
        type: string;
        constructor() {
            this.type = 'TextBlock';
        }
        props: TextBlockProps;
    };
    export class ContentControl {
        type: string;
        constructor() {
            this.type = 'ContentControl';
        }
        props: ContentControlProps;
    };
    export class Button {
        type: string;
        constructor() {
            this.type = 'Button';
        }
        props: ButtonProps;
    };
    export class CheckBox {
        type: string;
        constructor() {
            this.type = 'CheckBox';
        }
        props: CheckBoxProps;
    };
    export class Slider {
        type: string;
        constructor() {
            this.type = 'Slider';
        }
        props: SliderProps;
    };
    export class ProgressBar {
        type: string;
        constructor() {
            this.type = 'ProgressBar';
        }
        props: ProgressBarProps;
    };
    export class RichEditBox {
        type: string;
        constructor() {
            this.type = 'RichEditBox';
        }
        props: RichEditBoxProps;
    };
    export class TextBox {
        type: string;
        constructor() {
            this.type = 'TextBox';
        }
        props: TextBoxProps;
    };
    export class ListBox {
        type: string;
        constructor() {
            this.type = 'ListBox';
        }
        props: ListBoxProps;
    };
    export class ComboBox {
        type: string;
        constructor() {
            this.type = 'ComboBox';
        }
        props: ComboBoxProps;
    };
    export class CalendarDatePicker {
        type: string;
        constructor() {
            this.type = 'CalendarDatePicker';
        }
        props: CalendarDatePickerProps;
    };
    export class CalendarView {
        type: string;
        constructor() {
            this.type = 'CalendarView';
        }
        props: CalendarViewProps;
    };
    export class RelativePanel {
        type: string;
        constructor() {
            this.type = 'RelativePanel';
        }
        props: RelativePanelProps;
    };
    export class RepositionThemeTransition {
        type: string;
        constructor() {
            this.type = 'RepositionThemeTransition';
        }
        props: RepositionThemeTransitionProps;
    };
}

module XsrtJS {
    // UNDONE: a bit of a hack... I want the "App" to see what we define here, but we 
    // also call into random things defined on "App"... noodle on this more
    //
    declare var App: any;

    function refreshClicked() {
        xsrt.forceCleanReload();
    }

    export function displayError(errorMessage: string) {
        return (
            <Xaml.StackPanel>
                <Xaml.TextBlock fontFamily='Consolas' text={ errorMessage } />
                <Xaml.Button onClick={refreshClicked} content='Refresh' />
            </Xaml.StackPanel>
        );
    }

    function render(ev) {
        try {
            ev.view = JSON.stringify((App && App.render) ? App.render() : displayError('Error: App.render not found!'));
        }
        catch (e) {
            ev.view = JSON.stringify(displayError('Error: ' + e ));
        }
    }
    function command(ev) {
        try {
            var handler = App && App.eventHandlers && App.eventHandlers[ev.commandHandlerToken];
            if (handler) { handler(ev.sender, ev.eventArgs); }
        }
        catch (e) {
            // UNDONE: add exception pipe
        }
    }
    xsrt.addEventListener('render', render);
    xsrt.addEventListener('command', command);
    if (App && App.setInitialState) {
        if (!xsrt.isInitialized) {
            xsrt.isInitialized = true;
            App.setInitialState();
        }
    }
}

