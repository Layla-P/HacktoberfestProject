using System.Collections.Generic;

namespace HacktoberfestProject.Web.Models
{
    public class UserinfoViewModel
    {
        public string Username { get; set; }

        public List<RepositoryViewModel> Repositories {get;set;}
    }

    public class RepositoryViewModel
    {
        public string Name { get; set; }

        public string Owner { get; set; }

        public List<PullRequestViewModel> PullRequests { get; set; }

    }

    public class PullRequestViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}