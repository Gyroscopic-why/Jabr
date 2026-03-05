using System;
using System.Linq;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class EncryptionKey : IEncryptionKey
    {
        override public SetHelper Set => _setHelper;


        public class SetHelper : ISetHelper
        {
            private readonly EncryptionKey _reKey;
            private readonly SensetiveSetHelper _sensitiveSetHelper;

            public SetHelper(EncryptionKey reKey)
            {
                _reKey = reKey;
                _sensitiveSetHelper = new(_reKey);
            }


            public SensetiveSetHelper Sensitive => _sensitiveSetHelper;
            public class SensetiveSetHelper(EncryptionKey reKey)
            {
                private readonly EncryptionKey _reKey = reKey;


                public void PrAlphabet(string prAlphabet)
                        => _reKey._primaryAlphabet = prAlphabet;
                public void PrimaryAlphabet(string primaryAlphabet)
                        => _reKey._primaryAlphabet = primaryAlphabet;
                public bool SafePrAlphabet(string prAlphabet)
                {
                    if (!_reKey.IsPrimaryValid(prAlphabet)) return false;
                    _reKey._primaryAlphabet = prAlphabet;
                    return true;
                }
                public bool SafePrimaryAlphabet(string primaryAlphabet)
                {
                    if (!_reKey.IsPrimaryValid(primaryAlphabet)) return false;
                    _reKey._primaryAlphabet = primaryAlphabet;
                    return true;
                }



                public void ExAlphabet(string exAlphabet)
                        => _reKey._externalAlphabet = exAlphabet;
                public void ExternalAlphabet(string externalAlphabet)
                        => _reKey._externalAlphabet = externalAlphabet;
                public bool SafeExAlphabet(string exAlphabet)
                {
                    if (!_reKey.IsExternalValid(exAlphabet)) return false;
                    _reKey._externalAlphabet = exAlphabet;
                    return true;
                }
                public bool SafeExternalAlphabet(string externalAlphabet)
                {
                    if (!_reKey.IsExternalValid(externalAlphabet)) return false;
                    _reKey._externalAlphabet = externalAlphabet;
                    return true;
                }


                public void Shifts(List<Int16> shifts)
                {
                    _reKey._shifts.Clear();
                    _reKey._shifts.AddRange(shifts.Count > 0 ? shifts : [0]);
                }
                public bool SafeShifts(List<Int16> shifts)
                {
                    if (shifts.Max() > _reKey.ExLength) return false;
                    _reKey._shifts.Clear();
                    _reKey._shifts.AddRange(shifts);
                    return true;
                }
            }



            override public void Default()
            {
                _reKey._primaryNecessary = [.. DEFAULT.CHARACTERS.WITH_SPACE];
                _reKey._externalAllowed = [.. DEFAULT.CHARACTERS.WITHOUT_SPACE];

                _reKey._primaryMaxLength = _reKey._primaryNecessary.Count;
                _reKey._externalMaxLength = 8;
            }
            public override void ShiftCount(int count) => _reKey._shCount = count;



            public void Default(List<char> pNecessary, List<char> pAllowed, List<char> pBanned, Int32 pMaxLength,
                                List<char> eNecessary, List<char> eAllowed, List<char> eBanned, Int32 eMaxLength)
            {
                _reKey._primaryNecessary = [.. pNecessary];
                _reKey._externalNecessary = [.. eNecessary];

                _reKey._primaryAllowed = [.. pAllowed];
                _reKey._externalAllowed = [.. eAllowed];

                _reKey._primaryBanned = [.. pBanned];
                _reKey._externalBanned = [.. eBanned];

                _reKey._primaryMaxLength = pMaxLength;
                _reKey._externalMaxLength = eMaxLength;
            }
            public void Default(string pNecessary, string pAllowed, string pBanned, Int32 pMaxLength,
                                   string eNecessary, string eAllowed, string eBanned, Int32 eMaxLength)
                     => Default([.. pNecessary], [.. pAllowed], [.. pBanned], pMaxLength,
                                [.. eNecessary], [.. eAllowed], [.. eBanned], eMaxLength);

            public void Default(List<char> pNecessary, List<char> pAllowed, Int32 pMaxLength,
                                List<char> eNecessary, List<char> eAllowed, Int32 eMaxLength)
            {
                _reKey._primaryNecessary = [.. pNecessary];
                _reKey._externalNecessary = [.. eNecessary];

                _reKey._primaryAllowed = [.. pAllowed];
                _reKey._externalAllowed = [.. eAllowed];

                _reKey._primaryMaxLength = pMaxLength;
                _reKey._externalMaxLength = eMaxLength;
            }
            public void Default(string pNecessary, string pAllowed, Int32 pMaxLength,
                                   string eNecessary, string eAllowed, Int32 eMaxLength)
                     => Default([.. pNecessary], [.. pAllowed], pMaxLength,
                                [.. eNecessary], [.. eAllowed], eMaxLength);

            public void Default(Int32 pMaxLength, List<char> pNecessary, List<char> pBanned,
                                Int32 eMaxLength, List<char> eNecessary, List<char> eBanned)
            {
                _reKey._primaryNecessary = [.. pNecessary];
                _reKey._externalNecessary = [.. eNecessary];

                _reKey._primaryBanned = [.. pBanned];
                _reKey._externalBanned = [.. eBanned];

                _reKey._primaryMaxLength = pMaxLength;
                _reKey._externalMaxLength = eMaxLength;
            }
            public void Default(Int32 pMaxLength, string pNecessary, string pBanned,
                                Int32 eMaxLength, string eNecessary, string eBanned)
                     => Default(pMaxLength, [.. pNecessary], [.. pBanned],
                                eMaxLength, [.. eNecessary], [.. eBanned]);

            public void Default(Int32 pMaxLength, List<char> pBanned,
                                Int32 eMaxLength, List<char> eBanned)
            {
                _reKey._primaryBanned = [.. pBanned];
                _reKey._externalBanned = [.. eBanned];

                _reKey._primaryMaxLength = pMaxLength;
                _reKey._externalMaxLength = eMaxLength;
            }
            public void Default(Int32 pMaxLength, string pBanned,
                                Int32 eMaxLength, string eBanned)
                     => Default(pMaxLength, [.. pBanned],
                                eMaxLength, [.. eBanned]);

            public void Default(List<char> necessary, List<char> allowed, List<char> banned, Int32 maxLength)
                     => Default(necessary, allowed, banned, maxLength,
                                necessary, allowed, banned, maxLength);
            public void Default(string necessary, string allowed, string banned, Int32 maxLength)
                     => Default([.. necessary], [.. allowed], [.. banned], maxLength,
                                [.. necessary], [.. allowed], [.. banned], maxLength);

            public void Default(List<char> necessary, List<char> allowed, Int32 maxLength)
                     => Default(necessary, allowed, maxLength,
                                necessary, allowed, maxLength);
            public void Default(string necessary, string allowed, Int32 maxLength)
                     => Default([.. necessary], [.. allowed], maxLength,
                                [.. necessary], [.. allowed], maxLength);

            public void Default(Int32 maxLength, List<char> necessary, List<char> banned)
                     => Default(maxLength, necessary, banned,
                                maxLength, necessary, banned);
            public void Default(Int32 maxLength, string necessary, string banned)
                     => Default(maxLength, [.. necessary], [.. banned],
                                maxLength, [.. necessary], [.. banned]);

            public void Default(Int32 maxLength, List<char> banned)
                     => Default(maxLength, banned,
                                maxLength, banned);
            public void Default(Int32 maxLength, string banned)
                     => Default(maxLength, [.. banned],
                                maxLength, [.. banned]);
        }
    }
}