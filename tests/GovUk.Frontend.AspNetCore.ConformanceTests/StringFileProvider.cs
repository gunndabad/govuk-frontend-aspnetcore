using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public class StringFileProvider : IFileProvider
{
    private readonly Dictionary<string, (string Value, DateTimeOffset Created)> _values;

    public StringFileProvider()
    {
        _values = new Dictionary<string, (string Value, DateTimeOffset Created)>();
    }

    public void Add(string path, string value)
    {
        if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (!path.StartsWith("/"))
        {
            throw new ArgumentException($"{nameof(path)} must start with '/'.", nameof(path));
        }

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        _values.Add(path, (value, DateTimeOffset.Now));
    }

    public IDirectoryContents GetDirectoryContents(string subpath) => new NotFoundDirectoryContents();

    public IFileInfo GetFileInfo(string subpath)
    {
        if (_values.TryGetValue(subpath, out var entry))
        {
            return new StringFileInfo(subpath, subpath, entry.Value, entry.Created);
        }
        else
        {
            return new NotFoundFileInfo(subpath);
        }
    }

    public IChangeToken Watch(string filter) => NullChangeToken.Singleton;

    private class StringFileInfo : IFileInfo
    {
        private readonly string _value;
        private readonly DateTimeOffset _created;

        public StringFileInfo(string name, string physicalPath, string value, DateTimeOffset created)
        {
            Name = name;
            PhysicalPath = physicalPath;
            _value = value;
            _created = created;
        }

        public bool Exists => true;

        public long Length => _value.Length;

        public string PhysicalPath { get; }

        public string Name { get; }

        public DateTimeOffset LastModified => _created;

        public bool IsDirectory => false;

        public Stream CreateReadStream()
        {
            var bytes = Encoding.UTF8.GetBytes(_value);

            var ms = new MemoryStream();
            ms.Write(bytes);
            ms.Seek(0L, SeekOrigin.Begin);

            return ms;
        }
    }
}
