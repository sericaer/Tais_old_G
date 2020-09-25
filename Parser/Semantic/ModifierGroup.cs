﻿using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Syntax;

namespace Parser.Semantic
{
    internal class ModifierGroup
    {
        internal static ModifierGroup ParseItem(SyntaxItem item)
        {
            return SemanticParser.DoParser<ModifierGroup>(item);
        }

        internal double CalcValue()
        {
            double modifierValue = 0.0;
            if(modifiers != null)
            {
                modifierValue = modifiers.Where(x => x.condition.Rslt()).Sum(x => x.value);
            }

            var rslt = baseValue != null ? baseValue.Value + modifierValue : modifierValue;

            return rslt > 0 ? rslt : 0;
        }

        [SemanticProperty("base")]
        double? baseValue;

        [SemanticPropertyArray("modifier")]
        List<Modifier> modifiers;
    }

    internal class Modifier
    {
        internal static Modifier ParseItem(SyntaxItem item)
        {
            return SemanticParser.DoParser<Modifier>(item);
        }

        [SemanticProperty("value")]
        internal double value;

        [SemanticProperty("condition")]
        internal Condition condition;
    }
}