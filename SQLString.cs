using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleGeek
{
    public class SQLString
    {
        private static string Clean(string s)
        {
            return s.Replace("'", "''");
        }

        public static string CreateContent(int type, int status)
        {
            return string.Format("EXEC proc_LittleGeek_CreateContent {0}, '{1}'", type, status);
        }

        public static string CreateMedia(Media m)
        {
            return string.Format("EXEC proc_LittleGeek_CreateMedia {0}, {1}, '{2}'", m.ContentID, m.MediaType.GetHashCode(), Clean(m.FileName));
        }

        public static string CreateTitle(Media m)
        {
            return string.Format("EXEC proc_LittleGeek_CreateMediaInfo {0}, {1}, '{2}'", m.ID, Types.MediaInfoTypes.Title.GetHashCode(), Clean(m.Title));
        }

        public static string CreateCaption(Media m)
        {
            return string.Format("EXEC proc_LittleGeek_CreateMediaInfo {0}, {1}, '{2}'", m.ID, Types.MediaInfoTypes.Caption.GetHashCode(), Clean(m.Caption));
        }

        public static string CreateDescription(Media m)
        {
            return string.Format("EXEC proc_LittleGeek_CreateMediaInfo {0}, {1}, '{2}'", m.ID, Types.MediaInfoTypes.Description.GetHashCode(), Clean(m.Description));
        }

        public static string CreateMetadata(Media m)
        {
            return string.Format("EXEC proc_LittleGeek_CreateMediaInfo {0}, {1}, '{2}'", m.ID, Types.MediaInfoTypes.Metadata.GetHashCode(), Clean(m.Metadata));
        }

        public static string CreateMediaInfo(Media m, Types.MediaInfoTypes infoType, string info)
        {
            return string.Format("EXEC proc_LittleGeek_CreateMediaInfo {0}, {1}, '{2}'", m.ID, infoType.GetHashCode(), Clean(info));
        }
    }
}
