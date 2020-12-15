namespace GovUk.Frontend.AspNetCore
{
    public static class ComponentDefaults
    {
        public static class Button
        {
            public const bool Disabled = false;
            public const bool IsStartButton = false;
            public const bool PreventDoubleClick = false;
            public const string Type = "submit";
        }

        public static class DateInput
        {
            public const bool Disabled = false;
        }

        public static class Details
        {
            public const bool Open = false;
        }

        public static class ErrorMessage
        {
            public const string VisuallyHiddenText = "Error";
        }

        public static class ErrorSummary
        {
            public const string Title = "There is a problem";
        }

        public static class Fieldset
        {
            public static class Legend
            {
                public const bool IsPageHeading = false;
            }
        }

        public static class Input
        {
            public const bool Disabled = false;
            public const string Type = "text";
        }

        public static class Label
        {
            public const bool IsPageHeading = false;
        }

        public static class Panel
        {
            public const int HeadingLevel = 1;
        }

        public static class Select
        {
            public const bool Disabled = false;
        }

        public static class Tabs
        {
            public const string Title = "Contents";
        }

        public static class TextArea
        {
            public const bool Disabled = false;
            public const int Rows = 5;
        }
    }
}
