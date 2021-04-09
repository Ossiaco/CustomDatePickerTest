// Copyright (c) PlaceholderCompany. All rights reserved.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Program is program", Scope = "type", Target = "~T:Custom.DatePickerTest.iOS.Application")]
[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "dcbel is a brand", Scope = "namespaceanddescendants", Target = "~N:Custom.DatePickerTest.iOS")]
[assembly: SuppressMessage("Design", "CA1010:Collections should implement generic interface", Justification = "CustomRenderer for slider does not use any part of the Collection interface", Scope = "namespaceanddescendants", Target = "~N:Custom.DatePickerTest.iOS.Renderers")]
[assembly: SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "CustomRenderer for slider is not a collection", Scope = "namespaceanddescendants", Target = "~N:Custom.DatePickerTest.iOS.Renderers")]
[assembly: SuppressMessage("Documentation", "CA1200:Avoid using cref tags with a prefix", Justification = "I don't see a problem with cref being in documentation", Scope = "namespaceanddescendants", Target = "~N:Custom.DatePickerTest.iOS")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "This should not be a problem when debugging", Scope = "namespaceanddescendants", Target = "~N:Custom.DatePickerTest.iOS")]
