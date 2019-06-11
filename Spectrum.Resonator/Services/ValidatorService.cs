using Newtonsoft.Json;
using Spectrum.Resonator.Models;
using Spectrum.Resonator.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Spectrum.Resonator.Services
{
    public class ValidatorService : IValidatorService
    {
        private List<Validator> Validators { get; }

        public ValidatorService()
        {
            Validators = new List<Validator>();

            if (Directory.Exists("./_validators"))
            {
                var validatorFiles = Directory.GetFiles("./_validators", "*.json");

                foreach (var filePath in validatorFiles)
                    Validators.Add(JsonConvert.DeserializeObject<Validator>(File.ReadAllText(filePath)));
            }
        }

        public bool ValidateFileSystemPath(string path, string validatorName)
        {
            // FIXME: Will throw if validator doesn't exist; handle.
            var validator = Validators.First(v => v.Name == validatorName);

            foreach (var file in validator.RequiredFiles)
            {
                if (!File.Exists(Path.Combine(path, file)))
                    return false;
            }

            return true;
        }

        public bool ValidateZipArchive(string path, string validatorName)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            // FIXME: Will throw if validator doesn't exist; handle.
            var validator = Validators.First(v => v.Name == validatorName);

            try
            {
                using (var zipArchive = ZipFile.OpenRead(path))
                {
                    foreach (var file in validator.RequiredFiles)
                    {
                        if (zipArchive.Entries.FirstOrDefault(e => e.FullName == file) == null)
                            return false;
                    }
                }
            }
            catch (InvalidDataException)
            {
                return false;
            }

            return true;
        }
    }
}