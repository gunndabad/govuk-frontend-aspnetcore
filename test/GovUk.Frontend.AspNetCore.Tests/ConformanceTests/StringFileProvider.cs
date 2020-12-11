using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace GovUk.Frontend.AspNetCore.Tests.ConformanceTests
{
    public class StringFileProvider : IFileProvider
    {
        private readonly string _path;

        public StringFileProvider(string path, string initialValue = null)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!path.StartsWith("/"))
            {
                throw new ArgumentException($"{nameof(path)} must start with '/'.", nameof(path));
            }

            _path = path;
            Value = initialValue;
        }

        public string Value { get; set; }

        public IDirectoryContents GetDirectoryContents(string subpath) => new NotFoundDirectoryContents();

        public IFileInfo GetFileInfo(string subpath)
        {
            if (_path == subpath)
            {
                return new StringFileInfo(subpath, subpath, Value);
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

            public StringFileInfo(string name, string physicalPath, string value)
            {
                Name = name;
                PhysicalPath = physicalPath;
                _value = value;
            }

            public bool Exists => true;

            public long Length => _value.Length;

            public string PhysicalPath { get; }

            public string Name { get; }

            public DateTimeOffset LastModified => DateTimeOffset.Now;

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
}
