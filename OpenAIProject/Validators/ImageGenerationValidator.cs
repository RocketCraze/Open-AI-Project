namespace OpenAIProject.Validators
{
    using FluentValidation;

    using OpenAIProject.Models;

    public class ImageGenerationValidator : AbstractValidator<ImageGenerationAI>
    {
        public ImageGenerationValidator() 
        {
            this.RuleSet("Create", () =>
            {
                this.GeneralRules();
            });
        }

        private void GeneralRules()
        {
            this.RuleFor(_ => _.Prompt)
                .NotNull()
                .WithMessage("Prompt cannot be empty")
                .MinimumLength(1)
                .WithMessage("Prompt cannot be empty");
        }
    }
}
