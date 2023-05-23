using Microsoft.AspNetCore.Http;
using Shorti.Shared.Kernel.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Shorti.Shared.Kernel
{
    public class FileValidator : IFileValidator
    {
        private string[] _validFileTypes;
        private uint _maxFileSize;

        public FileValidator(FileValidatorSettings validationSettings)
        {
            ArgumentNullException.ThrowIfNull(validationSettings, nameof(validationSettings));

            _validFileTypes = validationSettings.ValidFileTypes;
            _maxFileSize = validationSettings.MaxFileSize;
        }

        public IEnumerable<ValidationResult> Validate(IFormFile file)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (file == null)
            {
                errors.Add(new ValidationResult("file равен null."));

                return errors;
            }

            if (_validFileTypes.Contains(Path.GetExtension(file!.FileName).ToLower()))
            {
                errors.Add(new ValidationResult("Файл имеет недопустимый тип."));
            }

            if (file.Length > _maxFileSize)
            {
                errors.Add(new ValidationResult("Файл превышает допустимый размер"));
            }

            return errors;
        }
    }
}
