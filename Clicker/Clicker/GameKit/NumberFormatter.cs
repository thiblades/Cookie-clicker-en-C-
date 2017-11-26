using System;

namespace Clicker.GameKit {
    public class NumberFormatter {
        // c.f. (http://bdl.oqlf.gouv.qc.ca/bdl/gabarit_bdl.asp?id=4445)
        private static readonly string[] Notations = {
            "",
            " million",
            " milliard",
            " billion",
            " billiard",
            " trillion",
            " trilliard",
            " quadrillion",
            " quadrilliard",
            " quintillion",
            " quintilliard",
            " sextillion",
            " sextilliard",
            " septillion",
            " septilliard",
            " octillion",
            " octilliard",
            " nonillion",
            " nonilliard",
            " décillion",
            " décilliard",
            " undécillion",
            " undécilliard",
            " duodécillion",
            " duodécilliard",
            " trédécillion",
            " trédécilliard",
            " quatuordécillion",
            " quatuordécilliard",
            " quindécillion",
            " quindécilliard",
        };

        private static readonly string[] ShortNotations = {
            "",
            "m",
            "M",
            "b",
            "B",
            "t",
            "T",
            "qa",
            "Qa",
            "qi",
            "Qi",
            "si",
            "Si",
            "se",
            "Se",
            "oc",
            "Oc",
            "no",
            "No",
            "dé",
            "Dé",
            "tr",
            "Tr",
            "qat",
            "Qat",
            "qit",
            "Qit",
        };

        private static string FormatInternal(double n, string[] notations, bool usePlural=true){
            int numberClass = 0;
            string notation = null;

            // Find in which class the number is
            if( n >= 1000000 && !Double.IsInfinity(n) ){
                n /= 1000;

                // Keep reducing the number until it becomes first class.
                while( n >= 1000 ){
                    n /= 1000;
                    numberClass++;
                }

                // If its class is so high we don't have a name for it, just bail.
                if( numberClass >= notations.Length )
                    return ":O";

                // Otherwise, record the class
                notation = notations[numberClass];
            }

            // Display the number in its preferred class. We round here to make
            // sure the displayed number will have at most three fractionnal digits.
            double Display = Math.Round(n * 1000) / 1000;

            if( notation == null ){ // No class: < 1 million
                return String.Format("{0}", Display);
            } else { // It has a class: use singular or plural as necessary.
                if( Display >= 2 && usePlural )
                    return String.Format("{0}{1}s", Display, notation);
                else
                    return String.Format("{0}{1}", Display, notation);
            }
        }

        public static string Format(double n) {
            return FormatInternal(n, Notations);
        }

        public static string FormatShort(double n) {
            return FormatInternal(n, ShortNotations, usePlural: false);
        }
    }
}
