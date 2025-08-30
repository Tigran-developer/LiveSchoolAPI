namespace WebAPI.Constants
{
    public static class Permissions
    {
        // Role-based permissions
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Teacher = "Teacher";
            public const string Pupil = "Pupil";
        }

        // Permission levels
        public static class Levels
        {
            public const string Read = "Read";
            public const string Write = "Write";
            public const string Delete = "Delete";
            public const string Manage = "Manage";
        }

        // Resource-based permissions
        public static class Resources
        {
            public const string Classes = "Classes";
            public const string Pupils = "Pupils";
            public const string Teachers = "Teachers";
            public const string Lessons = "Lessons";
            public const string Users = "Users";
            public const string System = "System";
        }
    }
}
