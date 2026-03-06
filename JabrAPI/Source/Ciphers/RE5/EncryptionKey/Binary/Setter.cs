using JabrAPI.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using static JabrAPI.RE5.EncryptionKey.SetHelper;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
        override public SetHelper Set => _setHelper;


        public class SetHelper : ISetHelper
        {
            private readonly BinaryKey _binKey;
            private readonly SensitiveSetHelper _sensitiveSetHelper;

            internal SetHelper(BinaryKey binKey)
            {
                _binKey = binKey;
                _sensitiveSetHelper = new(_binKey);
            }



            public SensitiveSetHelper Sensitive => _sensitiveSetHelper;
            public class SensitiveSetHelper
            {
                private readonly BinaryKey _binKey;

                internal SensitiveSetHelper(BinaryKey binKey)
                {
                    _binKey = binKey;
                }



                public void PrAlphabet(List<Byte> prAlphabet)
                {
                    _binKey._primaryAlphabet.Clear();
                    _binKey._primaryAlphabet.AddRange(prAlphabet);
                }
                public void PrimaryAlphabet(List<Byte> primaryAlphabet)
                {
                    _binKey._primaryAlphabet.Clear();
                    _binKey._primaryAlphabet.AddRange(primaryAlphabet);
                }
                public bool SafePrAlphabet(List<Byte> prAlphabet)
                {
                    if (!_binKey.IsValid.Primary(prAlphabet)) return false;
                    _binKey._primaryAlphabet.Clear();
                    _binKey._primaryAlphabet.AddRange(prAlphabet);
                    return true;
                }
                public bool SafePrimaryAlphabet(List<Byte> primaryAlphabet)
                {
                    if (!_binKey.IsValid.Primary(primaryAlphabet)) return false;
                    _binKey._primaryAlphabet.Clear();
                    _binKey._primaryAlphabet.AddRange(primaryAlphabet);
                    return true;
                }



                public void ExAlphabet(List<Byte> exAlphabet)
                {
                    _binKey._externalAlphabet.Clear();
                    _binKey._externalAlphabet.AddRange(exAlphabet);
                }
                public void ExternalAlphabet(List<Byte> externalAlphabet)
                {
                    _binKey._externalAlphabet.Clear();
                    _binKey._externalAlphabet.AddRange(externalAlphabet);
                }
                public bool SafeExAlphabet(List<Byte> exAlphabet)
                {
                    if (!_binKey.IsValid.External(exAlphabet)) return false;
                    _binKey._externalAlphabet.Clear();
                    _binKey._externalAlphabet.AddRange(exAlphabet);
                    return true;
                }
                public bool SafeExternalAlphabet(List<Byte> externalAlphabet)
                {
                    if (!_binKey.IsValid.External(externalAlphabet)) return false;
                    _binKey._externalAlphabet.Clear();
                    _binKey._externalAlphabet.AddRange(externalAlphabet);
                    return true;
                }



                public void Shifts(List<Byte> shifts)
                {
                    _binKey._shifts.Clear();
                    _binKey._shifts.AddRange(shifts.Count > 0 ? shifts : [0]);
                }
                public bool SafeShifts(List<Byte> shifts)
                {
                    if (shifts.Max() > _binKey.ExLength) return false;
                    _binKey._shifts.Clear();
                    _binKey._shifts.AddRange(shifts.Count > 0 ? shifts : [0]);
                    return true;
                }
                public void Shift(Byte shift)
                {
                    _binKey._shifts.Clear();
                    _binKey._shifts.Add(shift);
                }
                public bool SafeShifts(Byte shift)
                {
                    if (_binKey.Shifts.Max() > _binKey.ExLength) return false;
                    _binKey._shifts.Clear();
                    _binKey._shifts.Add(shift);
                    return true;
                }
            }



            public override void ShiftCount(Int32 count) => _binKey._shCount = count;
            public override void Default()
            {
                _binKey._compactedPrMaxLength = 255;
                _binKey._compactedExMaxLength = 7;
            }



            public void Default(Byte compactedPrMaxLength, Byte compactedExMaxLength)
            {
                _binKey._compactedPrMaxLength = compactedPrMaxLength;
                _binKey._compactedExMaxLength = compactedExMaxLength;
            }

            public void DefaultOnlyEx(Byte compactedExMaxLength)
                => _binKey._compactedExMaxLength = compactedExMaxLength;
            public void DefaultOnlyPr(Byte compactedPrMaxLength)
                => _binKey._compactedPrMaxLength = compactedPrMaxLength;
        }
    }
}