using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField]Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach(var dis in and)
            {
                if(!dis.Check(evaluators)){return false; }

            }
            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField]Predicate[] or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach(var pred in or)
                {
                    if(pred.Check(evaluators))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] string predicate;
            [SerializeField] string[] praram;

            [SerializeField]bool negate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach(var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, praram);

                    if(result == null)
                    {
                        continue;
                    }
                    
                       if(result ==negate) return false;
                }

                return true;
            }
        }
       
       
    }
}
