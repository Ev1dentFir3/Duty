using System.Collections.Generic;
using JetBrains.Annotations;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace EFG.Duty.RocketExtended;

public abstract class RocketCommandWithTranslations : IRocketCommand
{
    public abstract AllowedCaller AllowedCaller { get; }
    public abstract string Name { get; }
    public abstract string Help { get; }
    public abstract string Syntax { get; }
    public virtual List<string> Aliases => new();
    public virtual List<string> Permissions => new() { Name };

    [UsedImplicitly] public IndexedTranslations Translator { get; }

    protected RocketCommandWithTranslations(TranslationList translations, IEnumerable<string> requiredTranslationsRegex)
    {
        Translator = new IndexedTranslations(translations, requiredTranslationsRegex);
    }

    [UsedImplicitly]
    public virtual void ReloadTranslations(TranslationList newTranslations)
    {
        Translator.ReloadTranslations(newTranslations);
    }

    [UsedImplicitly]
    protected virtual string Translate(string translationKey, params object[] placeholder)
    {
        return Translator.Translate(translationKey, placeholder);
    }

    [UsedImplicitly]
    protected virtual void SendMessage(IRocketPlayer player, string translationKey, string chatColor,
        params object[] placeholder)
    {
        UnturnedChat.Say(player, Translate(translationKey, placeholder),
            UnturnedChat.GetColorFromName(chatColor, Color.red));
    }

    [UsedImplicitly]
    protected virtual void SendMessage(IRocketPlayer player, string translationKey, params object[] placeholder)
    {
        UnturnedChat.Say(player, Translate(translationKey, placeholder));
    }

    [UsedImplicitly]
    protected virtual void SendMessage(string translationKey, string chatColor, params object[] placeholder)
    {
        UnturnedChat.Say(Translate(translationKey, placeholder), UnturnedChat.GetColorFromName(chatColor, Color.red));
    }

    public abstract void Execute(IRocketPlayer caller, string[] command);
}