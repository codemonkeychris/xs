declare var host: any;
declare var xsrt: any;
declare module XsrtJS {
}
declare module React {
    function createElement(ctor: any, members: any): any;
}
declare module Xaml {
    interface FrameworkElementProps {
        name?: string;
        width?: number;
        height?: number;
    }
    interface ViewboxProps extends FrameworkElementProps {
        stretch?: string;
    }
    class Viewbox {
        type: string;
        constructor();
        props: ViewboxProps;
    }
    class ScrollViewer {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class GridView {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class ListView {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class Grid {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class StackPanel {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class Image {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class TextBlock {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class Button {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class CheckBox {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class Slider {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class ProgressBar {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class RichEditBox {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class TextBox {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class ListBox {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class ComboBox {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class CalendarDatePicker {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class CalendarView {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class RelativePanel {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
    class RepositionThemeTransition {
        type: string;
        constructor();
        props: FrameworkElementProps;
    }
}
