using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Linq;

namespace ManicTime.Api.Client.Commands
{
    public class CommandValidator
    {
        private readonly List<Func<CommandResult, IEnumerable<string>>> _validators = new();

        public string Validate(CommandResult commandResult) =>
            string.Join(Environment.NewLine, _validators.SelectMany(validate => validate(commandResult)));

        public CommandValidator AddConditionalRequired(string conditionAlias, params string[] requiredAliases)
        {
            _validators.Add(validate);
            return this;

            IEnumerable<string> validate(CommandResult commandResult)
            {
                bool conditionMet = conditionAlias.StartsWith("!")
                    ? !commandResult.Children.Contains(conditionAlias.TrimStart('!'))
                    : commandResult.Children.Contains(conditionAlias);
                if (conditionMet)
                {
                    foreach (string requiredAlias in requiredAliases)
                    {
                        if (!commandResult.Children.Contains(requiredAlias))
                            yield return $"Option '{requiredAlias}' is required when option '{conditionAlias.TrimStart('!')}' is {(conditionAlias.StartsWith("!") ? "not " : "")}used";
                    }
                }
            }
        }

        public CommandValidator AddConditionalNotAllowed(string conditionAlias, params string[] notAllowedAliases)
        {
            _validators.Add(validate);
            return this;

            IEnumerable<string> validate(CommandResult commandResult)
            {
                bool conditionMet = conditionAlias.StartsWith("!")
                    ? !commandResult.Children.Contains(conditionAlias.TrimStart('!'))
                    : commandResult.Children.Contains(conditionAlias);
                if (conditionMet)
                {
                    foreach (string requiredAlias in notAllowedAliases)
                    {
                        if (commandResult.Children.Contains(requiredAlias))
                            yield return $"Option '{requiredAlias}' is not allowed when option '{conditionAlias.TrimStart('!')}' is {(conditionAlias.StartsWith("!") ? "not " : "")}used";
                    }
                }
            }
        }

        public CommandValidator AddAnyRequired(params string[] anyRequriedAliases)
        {
            _validators.Add(validate);
            return this;
            
            IEnumerable<string> validate(CommandResult commandResult)
            {
                if (!anyRequriedAliases.Any(a => commandResult.Children.Contains(a)))
                {
                    yield return $"Options {string.Join(" or ", anyRequriedAliases.Select(a => $"'{a}'"))} are required";
                }
            }
        }

        public CommandValidator AddAllRequired(params string[] allRequriedAliases)
        {
            _validators.Add(validate);
            return this;

            IEnumerable<string> validate(CommandResult commandResult)
            {
                foreach (string requiredAlias in allRequriedAliases)
                {
                    if (!commandResult.Children.Contains(requiredAlias))
                        yield return $"Option '{requiredAlias}' is required";
                }
            }
        }
    }
}
