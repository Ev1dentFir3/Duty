using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EFG.Duty.Utilities;
using JetBrains.Annotations;
using Rocket.API.Collections;

namespace EFG.Duty.RocketExtended;

[UsedImplicitly]
public class IndexedTranslations : IReadOnlyDictionary<string, string>
{
    [UsedImplicitly] protected Dictionary<string, string> Translations = new(StringComparer.OrdinalIgnoreCase);

    public IndexedTranslations(TranslationList translations, IEnumerable<string> requiredTranslationsRegex)
    {
        foreach (var translation in requiredTranslationsRegex)
        {
            var toIndex = translations.Where(k => Regex.IsMatch(k.Id, $"^{translation}$", RegexOptions.IgnoreCase))
                .ToList();

            if (toIndex.Count == 0)
            {
                Logging.Write("IndexedTranslations..cctor",
                    $"WARNING! No translation matching {translation} was found in the translation list! Please add it to the translations.",
                    ConsoleColor.Yellow);
                continue;
            }

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var index in toIndex)
            {
                if (Translations.ContainsKey(index.Id))
                    continue;

                Translations.Add(index.Id, index.Value);
            }
        }
    }

    public void ReloadTranslations(TranslationList newTranslations)
    {
        foreach (var key in Translations.Keys.ToList())
        {
            var toUpdate = newTranslations.FirstOrDefault(k => k.Id.Equals(key, StringComparison.OrdinalIgnoreCase));

            if (toUpdate == null)
                continue;

            Translations[key] = toUpdate.Value;
        }
    }

    public string Translate(string translationKey, params object?[] args)
    {
        if (!TryGetValue(translationKey, out var translation))
            return translationKey;

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            translation = translation.Replace($"{{{i}}}", arg?.ToString() ?? "NULL");
        }

        return translation;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return Translations.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Translations.GetEnumerator();
    }

    public int Count => Translations.Count;

    public bool ContainsKey(string key)
    {
        return Translations.ContainsKey(key);
    }

    public bool TryGetValue(string key, out string value)
    {
        return Translations.TryGetValue(key, out value);
    }

    public string this[string key] => Translations[key];

    public IEnumerable<string> Keys => Translations.Keys;
    public IEnumerable<string> Values => Translations.Values;
}