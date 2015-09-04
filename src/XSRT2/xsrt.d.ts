declare var host: any;
declare var xsrt: any;
declare module React {
    function createElement(ctor: any, members: any): any;
    function __spread(target: any, record1: any, record2: any): any;
}
declare module JSX {
    interface ElementAttributesProperty {
        props: any;
    }
}
declare module Xaml {
    interface Geopoint {
        latitude: number;
        longitude: number;
    }
    interface MapItemsControlProps {
        name?: string;
        items?: any;
    }
    interface MapControlProps extends ControlProps {
        businessLandmarksVisible?: boolean;
        center?: Geopoint;
        desiredPitch?: number;
        heading?: number;
        landmarksVisible?: boolean;
        mapServiceToken: string;
        pedestrianFeaturesVisible?: boolean;
        trafficFlowVisible?: boolean;
        transitFeaturesVisible?: boolean;
        zoomLevel?: number;
        onActualCameraChanged?: any;
        onActualCameraChanging?: any;
        onCenterChanged?: any;
        onCustomExperienceChanged?: any;
        onHeadingChanged?: any;
        onLoadingStatusChanged?: any;
        onMapDoubleTapped?: any;
        onMapElementClick?: any;
        onMapElementPointerEntered?: any;
        onMapElementPointerExited?: any;
        onMapHolding?: any;
        onMapTapped?: any;
        onPitchChanged?: any;
        onTargetCameraChanged?: any;
        onTransformOriginChanged?: any;
        onZoomLevelChanged?: any;
    }
    interface ShapeProps extends FrameworkElementProps {
        fill?: string;
        stroke?: string;
        strokeThickness?: number;
    }
    interface EllipseProps extends ShapeProps {
    }
    interface RectangleProps extends ShapeProps {
    }
    interface MapControlAttachedProps {
        map$location?: Geopoint;
    }
    interface ScrollViewerAttachedProps {
        scrollViewer$verticalScrollBarVisibility?: string;
        scrollViewer$horizontalScrollBarVisibility?: string;
    }
    interface GridAttachedProps {
        grid$row?: number;
        grid$rowSpan?: number;
        grid$column?: number;
        grid$columnSpan?: number;
    }
    interface AccessibilityProps {
        automationId?: string;
        acc$helpText?: string;
        acc$labeledBy?: string;
        acc$liveSetting?: string;
        acc$name?: string;
    }
    interface RelativePanelAttachedProps {
        relative$above?: string;
        relative$alignBottomWith?: string;
        relative$alignBottomWithPanel?: boolean;
        relative$alignHorizontalCenterWith?: string;
        relative$alignLeftWith?: string;
        relative$alignLeftWithPanel?: boolean;
        relative$alignRightWith?: string;
        relative$alignRightWithPanel?: boolean;
        relative$alignTopWith?: string;
        relative$alignTopWithPanel?: boolean;
        relative$alignVerticalCenterWith?: string;
        relative$alignVerticalCenterWithPanel?: boolean;
        relative$below?: string;
        relative$leftOf?: string;
        relative$rightOf?: string;
    }
    interface UIElementProps {
        style?: {};
        opacity?: number;
        onKeyDown?: any;
        onKeyUp?: any;
    }
    interface FrameworkElementProps extends UIElementProps, ScrollViewerAttachedProps, GridAttachedProps, AccessibilityProps, RelativePanelAttachedProps, MapControlAttachedProps {
        name?: string;
        width?: number;
        height?: number;
        horizontalAlignment?: string;
        verticalAlignment?: string;
        margin?: string;
    }
    interface ViewboxProps extends FrameworkElementProps {
        stretch?: string;
        stretchDirection?: string;
        child?: any;
    }
    interface TextBlockProps extends FrameworkElementProps {
        foreground?: string;
        text?: string;
        fontFamily?: string;
        fontSize?: number;
        fontWeight?: string;
    }
    interface ImageProps extends FrameworkElementProps {
        source?: string;
    }
    interface TextBoxSharedProps extends ControlProps {
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
        onTextChanged?: any;
    }
    interface RichEditBoxProps extends TextBoxSharedProps {
    }
    interface TextBoxProps extends TextBoxSharedProps {
        selectionLength?: number;
        selectionStart?: number;
    }
    interface ItemsControlProps extends ControlProps {
        itemsSource?: any;
        itemContainerTransitions?: any;
    }
    interface SelectorProps extends ItemsControlProps {
        selectedItem?: any;
        onSelectionChanged?: any;
    }
    interface ListViewBaseProps extends SelectorProps {
    }
    interface ListViewProps extends ListViewBaseProps {
    }
    interface GridViewProps extends ListViewBaseProps {
    }
    interface ComboBoxProps extends SelectorProps {
    }
    interface ListBoxProps extends SelectorProps {
    }
    interface RangeBaseProps extends ControlProps {
        minimum?: number;
        maximum?: number;
        value?: number;
        onValueChanged?: any;
    }
    interface ButtonProps extends ButtonBaseProps {
    }
    interface ButtonBaseProps extends ControlProps {
    }
    interface CalendarDatePickerProps extends ControlProps {
        date?: string;
        onCalendarViewDayItemChanging?: any;
        onClosed?: any;
        onDateChanged?: any;
        onOpened?: any;
    }
    interface CalendarViewProps extends ControlProps {
        minDate?: string;
        maxDate?: string;
        onCalendarViewDayItemChanging?: any;
        onSelectedDatesChanged?: any;
    }
    interface RelativePanelProps extends PanelProps {
    }
    interface RepositionThemeTransitionProps {
    }
    interface ProgressBarProps extends RangeBaseProps {
    }
    interface SliderProps extends RangeBaseProps {
    }
    interface CheckBoxProps extends ButtonBaseProps {
        isChecked?: boolean;
        onChecked?: any;
    }
    interface ScrollViewerProps extends ContentControlProps {
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
        leftHeader?: string;
        topHeader?: string;
        topLeftHeader?: string;
        verticalScrollBarVisibility?: string;
        verticalScrollMode?: string;
        verticalSnapPointsAlignment?: string;
        verticalSnapPointsType?: string;
        zoomMode?: string;
        zoomSnapPointsType?: string;
    }
    interface ButtonBaseProps extends ContentControlProps {
        onClick?: any;
    }
    interface ContentControlProps extends ControlProps {
        content?: any;
    }
    interface ControlProps extends FrameworkElementProps {
        background?: string;
        foreground?: string;
        fontFamily?: string;
        fontSize?: number;
        fontWeight?: string;
    }
    interface PanelProps extends FrameworkElementProps {
        background?: string;
        childrenTransitions?: any;
    }
    interface StackPanelProps extends PanelProps {
        orientation?: string;
    }
    interface GridProps extends PanelProps {
        rows?: [string];
        columns?: [string];
    }
    class MapItemsControl {
        type: string;
        constructor();
        props: MapItemsControlProps;
    }
    class MapControl {
        type: string;
        constructor();
        props: MapControlProps;
    }
    class Ellipse {
        type: string;
        constructor();
        props: EllipseProps;
    }
    class Rectangle {
        type: string;
        constructor();
        props: RectangleProps;
    }
    class Viewbox {
        type: string;
        constructor();
        props: ViewboxProps;
    }
    class ScrollViewer {
        type: string;
        constructor();
        props: ScrollViewerProps;
    }
    class GridView {
        type: string;
        constructor();
        props: GridViewProps;
    }
    class ListView {
        type: string;
        constructor();
        props: ListViewProps;
    }
    class Grid {
        type: string;
        constructor();
        props: GridProps;
    }
    class StackPanel {
        type: string;
        constructor();
        props: StackPanelProps;
    }
    class Image {
        type: string;
        constructor();
        props: ImageProps;
    }
    class TextBlock {
        type: string;
        constructor();
        props: TextBlockProps;
    }
    class ContentControl {
        type: string;
        constructor();
        props: ContentControlProps;
    }
    class Button {
        type: string;
        constructor();
        props: ButtonProps;
    }
    class CheckBox {
        type: string;
        constructor();
        props: CheckBoxProps;
    }
    class Slider {
        type: string;
        constructor();
        props: SliderProps;
    }
    class ProgressBar {
        type: string;
        constructor();
        props: ProgressBarProps;
    }
    class RichEditBox {
        type: string;
        constructor();
        props: RichEditBoxProps;
    }
    class TextBox {
        type: string;
        constructor();
        props: TextBoxProps;
    }
    class ListBox {
        type: string;
        constructor();
        props: ListBoxProps;
    }
    class ComboBox {
        type: string;
        constructor();
        props: ComboBoxProps;
    }
    class CalendarDatePicker {
        type: string;
        constructor();
        props: CalendarDatePickerProps;
    }
    class CalendarView {
        type: string;
        constructor();
        props: CalendarViewProps;
    }
    class RelativePanel {
        type: string;
        constructor();
        props: RelativePanelProps;
    }
    class RepositionThemeTransition {
        type: string;
        constructor();
        props: RepositionThemeTransitionProps;
    }
}
declare module XsrtJS {
    function displayError(errorMessage: string): any;
}
