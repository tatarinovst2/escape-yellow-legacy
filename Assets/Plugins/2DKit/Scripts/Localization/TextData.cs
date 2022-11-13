using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Settings;

namespace Kit.Localization
{
    [System.Serializable]
    public class TextData
    {
        [SerializeField]
        [TextArea]
        private string _eng = "";
        public string ENG { get { return _eng; } }

        [SerializeField]
        [TextArea]
        private string _rus = "";
        public string RUS { get { return _rus; } }

        public string StringForCurrentLanguage()
        {
            Setting languageSetting = SettingsScript.Instance.SettingWithName("Language");

            if (languageSetting == null)
            {
                Debug.LogError("Language setting is not found");
                return "";
            }

            return StringFor((Language)languageSetting.CurrentValue);
        }

        public string StringFor(Language language)
        {
            if (language == Language.Russian)
            {
                return RUS;
            }
            else
            {
                return ENG;
            }
        }

        public TextData()
        {

        }

        public TextData(string ENG, string RUS)
        {
            _eng = ENG;
            _rus = RUS;
        }

        public TextData(string ALL)
        {
            _eng = ALL;
            _rus = ALL;
        }

        public TextData(List<string> datasByLanguage)
        {
            _eng = datasByLanguage[0];
            _rus = datasByLanguage[1];
        }

        public static List<Language> ReturnLanguages()
        {
            List<Language> languages = new List<Language>();

            languages.Add(Language.English);
            languages.Add(Language.Russian);

            return languages;
        }

        public bool IsTheSame(TextData textData)
        {
            if (this.RUS != textData.RUS)
            {
                return false;
            }

            if (this.ENG != textData.ENG)
            {
                return false;
            }

            return true;
        }

        public int PagesCountFor()
        {
            Setting languageSetting = SettingsScript.Instance.SettingWithName("Language");

            if (languageSetting == null)
            {
                Debug.LogError("Language setting is not found");
                return 1;
            }

            return PagesCountFor((Language)languageSetting.CurrentValue);
        }

        public int PagesCountFor(Language language)
        {
            string languageString = StringFor(language);

            string[] stringSeparators = new string[] { "<page>" };

            string[] pages = languageString.Split(stringSeparators, System.StringSplitOptions.None);

            return pages.Length;
        }

        public TextData ExtractPage(int pageNumber)
        {
            List<string> stringPages = new List<string>();

            foreach (Language language in ReturnLanguages())
            {
                string languageString = StringFor(language);

                string[] stringSeparators = new string[] { "<page>" };

                string[] pages = languageString.Split(stringSeparators, System.StringSplitOptions.None);

                if (pageNumber < pages.Length)
                {
                    stringPages.Add(pages[pageNumber]);
                }
                else
                {
                    stringPages.Add("");
                }
            }

            return new TextData(stringPages);
        }
    }
}