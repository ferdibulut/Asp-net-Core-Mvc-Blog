using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProgrammersBlog.Shared.Utilities.Helpers.Abstract;

namespace ProgrammersBlog.Shared.Utilities.Helpers.Concrete
{
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IHostingEnvironment _environment;//Calışma ortamımızı alıyoruz
        private readonly IOptionsMonitor<T> _options;//IOptionsSnapshot aynısı
        private readonly string _section;//appsetting icindeli aboutuspage gibi
        private readonly string _file; //hangi dosya,appsetting yada başkası mysettings yada appsetting.development.json

        public WritableOptions(IHostingEnvironment environment, IOptionsMonitor<T> options, string section, string file)
        {
            _environment = environment;
            _options = options;
            _section = section;
            _file = file;
        }

        public T Value { get; }//aboutuspage gibi
        public T Get(string name) => _options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var fileProvider = _environment.ContentRootFileProvider;//programersblog.mvc sitemizin yolunu alıyoruz
            var fileInfo = fileProvider.GetFileInfo(_file);//Gelen dosyanın bilgilerini alıyoruz
            var physicalPath = fileInfo.PhysicalPath;

            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));//alınan bilgileri objeye ceviriyoruz
            var sectionObject = jObject.TryGetValue(_section, out JToken section) ?
                JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

            applyChanges(sectionObject);

            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }
    }
}
