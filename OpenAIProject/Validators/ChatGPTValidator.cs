namespace OpenAIProject.Validators
{
    using FluentValidation;

    using OpenAIProject.Models;

    public class ChatGPTValidator : AbstractValidator<ChatGPTMessage>
    {
        public ChatGPTValidator() 
        {
            this.RuleSet("Create", () =>
            {
                this.GeneralRules();
            });
        }

        private void GeneralRules()
        {
            this.RuleFor(_ => _.role)
                .NotNull()
                .WithMessage("Role cannot be empty")
                .MinimumLength(1)
                .WithMessage("Role cannot be empty")
                .MaximumLength(50);

            this.RuleFor(_ => _.content)
                .NotNull()
                .WithMessage("Question cannot be empty")
                .MinimumLength(1)
                .WithMessage("Question cannot be empty");
        }
    }
}
