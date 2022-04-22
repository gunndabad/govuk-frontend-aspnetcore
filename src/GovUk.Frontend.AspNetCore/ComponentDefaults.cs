namespace GovUk.Frontend.AspNetCore
{
    public static class ComponentDefaults
    {
        public static class Breadcrumbs
        {
            public const bool CollapseOnMobile = false;
        }

        public static class DateInput
        {
            public const bool Disabled = false;
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
