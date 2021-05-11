using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [System.Serializable]
        class Predicate
        {

        }
        [SerializeField]string predicate;
        [SerializeField]string[] praram;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach(var evaluator in evaluators)
            {
                bool? result = evaluator.Evaluate(predicate, praram);

                if(result == null)
                {
                    continue;
                }
                if(result == false)return false;

               
                
            }

            return true;
        }
       
    }
}
