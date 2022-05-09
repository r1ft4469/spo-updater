using System;

namespace Aki.Launcher.Models
{
    public class ReleaseNotes
    {
        public string url { get; set; }
        public string assets_url { get; set; }
        public string upload_url { get; set; }
        public string html_url { get; set; }
        public int id { get; set; }
        public object author { get; set; }
        public string node_id { get; set; }
        public string tag_name { get; set; }
        public string target_commitish { get; set; }
        public string name { get; set; }
        public bool draft { get; set; }
        public bool prerelease { get; set; }
        public DateTime created_at { get; set; }
        public DateTime published_at { get; set; }
        public object[] assets { get; set; }
        public string tarball_url { get; set; }
        public string zipball_url { get; set; }
        public string body { get; set; }
    }


    public class Commit
    {
        public string sha { get; set; }
        public string node_id { get; set; }
        public object commit { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string comments_url { get; set; }
        public object author { get; set; }
        public object committer { get; set; }
        public object[] parents { get; set; }
        public object stats { get; set; }
        public object[] files { get; set; }
    }
}
