namespace GovUk.Frontend.AspNetCore
{
    public static class ComponentDefaults
    {
        public static class Accordion
        {
            public const int HeadingLevel = 2;
        }

        public static class Breadcrumbs
        {
            public const bool CollapseOnMobile = false;
        }

        public static class Button
        {
            public const bool Disabled = false;
            public const bool IsStartButton = false;
            public const bool PreventDoubleClick = false;
            public const string Type = "submit";
        }

        public static class ErrorMessage
        {
            public const string VisuallyHiddenText = "Error";
        }

        public static class ErrorSummary
        {
            public const string Title = "There is a problem";
        }

        public static class Input
        {
            public const string Type = "text";
        }

        public static class Panel
        {
            public const int HeadingLevel = 1;
        }

        public static class Tabs
        {
            public const string Title = "Contents";
        }

        public static class TextArea
        {
            public const int Rows = 5;
        }
    }
}
