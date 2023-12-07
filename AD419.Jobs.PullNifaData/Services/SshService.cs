using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Microsoft.Extensions.Options;

namespace AD419.Jobs.PullNifaData.Services;
public interface ISshService
{
    IEnumerable<string> ListFiles(string directory);
    void PlaceFile(string contents, string path);
    void MoveFile(string origPath, string newPath);
    MemoryStream DownloadFile(string fileName);
}

public class SshService : ISshService
{
    private readonly SshConnectionInfo _sshConnectionInfo;

    public SshService(IOptions<SshConnectionInfo> sshConnectionInfo)
    {
        _sshConnectionInfo = sshConnectionInfo.Value;
    }

    public void PlaceFile(string contents, string path)
    {
        using var client = GetScpClient();
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(contents));
        client.Upload(ms, path);
    }

    public IEnumerable<string> ListFiles(string directory)
    {
        using var client = GetSftpClient();
        return client.ListDirectory(directory).Select(x => x.FullName).ToArray();
    }

    private SftpClient GetSftpClient()
    {
        var client = new SftpClient(_sshConnectionInfo.Url, _sshConnectionInfo.Port, _sshConnectionInfo.Name, _sshConnectionInfo.Password);
        client.Connect();
        return client;
    }

    // for running shell commands
    private SshClient GetSshClient()
    {
        var client = new SshClient(_sshConnectionInfo.Url, _sshConnectionInfo.Port, _sshConnectionInfo.Name, _sshConnectionInfo.Password);
        client.Connect();
        return client;
    }

    // for file transfer
    private ScpClient GetScpClient()
    {
        var client = new ScpClient(_sshConnectionInfo.Url, _sshConnectionInfo.Port, _sshConnectionInfo.Name, _sshConnectionInfo.Password);
        client.Connect();
        return client;
    }

    public MemoryStream DownloadFile(string fileName)
    {
        using var client = GetSftpClient();
        var stream = new MemoryStream();
        client.DownloadFile(fileName, stream);

        return stream;
    }

    public void MoveFile(string origPath, string newPath)
    {
        using var client = GetSftpClient();
        client.RenameFile(origPath, newPath);
    }
}

public class SshConnectionInfo
{
    public string Url { get; set; } = "";
    public int Port { get; set; } = 22;
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";
}
