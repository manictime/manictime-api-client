namespace ManicTime.Api.Client.Commands
{
    public static class ApiCommandValidatorExtensions
    {
        public static CommandValidator AddApiAuthValidators(this CommandValidator commandValidator)
        {
            commandValidator.AddAnyRequired("--server-url", "--access-token");
            commandValidator.AddConditionalRequired("--password", "--username");
            commandValidator.AddConditionalNotAllowed("--access-token", "--username", "--password");
            commandValidator.AddConditionalNotAllowed("!--server-url", "--username", "--password");
            return commandValidator;
        }
    }
}