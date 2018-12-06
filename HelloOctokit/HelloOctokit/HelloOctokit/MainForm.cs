using System;
using System.IO;
using System.Collections;
using Eto.Forms;
using Eto.Drawing;
using Octokit;
using Octokit.Internal; // InMemoryCredentialStore
using Octokit.Reactive; // ObservableRepositoriesClient

namespace HelloOctokit
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            GetRepos();
        }
        public void GetRepos() {
            //var github = new GitHubClient(new ProductHeaderValue("GitHubClient"));
            var credential = new Credentials("username", "password"); // GitHubユーザ名とパスワード
            var github = new GitHubClient(
                new ProductHeaderValue("GitHubClient_ytyaru"), 
                new InMemoryCredentialStore(credential));
            var reposObserver = new ObservableRepositoriesClient(github);
            
            using (FileStream fs = new FileStream("/tmp/work/repos.tsv", System.IO.FileMode.OpenOrCreate)) {
                using (StreamWriter writer = new StreamWriter(fs)) {
                    writer.WriteLine(String.Join("\t", new string[] { "Id","Name","Size","Description","Homepage","CreatedAt","UpdatedAt" }));
                    reposObserver.GetAllForUser("ytyaru").Subscribe(
                        i => {
                            writer.WriteLine(String.Join("\t", new string[] { i.Id.ToString(),i.Name.ToString(),i.Size.ToString(),i.Description,i.Homepage,i.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),i.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss") }));
                            //writer.WriteLine(i.Id + "\t" + i.Name + "\t" + i.Size + "\t" + i.Description + "\t" + i.Homepage + "\t" + i.CreatedAt + "\t" + i.PushedAt + "\t" + i.UpdatedAt);
                        }
                    );
                    writer.Flush();
                }
            }
            ;
        }
    }
}