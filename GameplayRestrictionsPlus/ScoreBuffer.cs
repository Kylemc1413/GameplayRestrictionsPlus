using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayRestrictionsPlus
{
    class ScoreBuffer
    {
        private NoteCutInfo _noteCutInfo;
        private SaberAfterCutSwingRatingCounter _afterCutSwingRatingCounter;
        public Action<ScoreBuffer> didFinishEvent;

        public ScoreBuffer(NoteCutInfo noteCutInfo, SaberAfterCutSwingRatingCounter afterCutSwingRatingCounter)
        {
            _noteCutInfo = noteCutInfo;
            _afterCutSwingRatingCounter = afterCutSwingRatingCounter;
            _afterCutSwingRatingCounter.didFinishEvent += AfterCutSwingRatingCounter_didFinishEvent;
        }

        private void AfterCutSwingRatingCounter_didFinishEvent(SaberAfterCutSwingRatingCounter afterCutRating)
        {
            _afterCutSwingRatingCounter.didFinishEvent -= AfterCutSwingRatingCounter_didFinishEvent;
            didFinishEvent(this);
        }

        public int returnScore()
        {
            int before = 0;
            int after = 0;
            int acc = 0;
            ScoreController.ScoreWithoutMultiplier(_noteCutInfo, _afterCutSwingRatingCounter, out before, out after, out acc);
            return before + after;

        }
    }
}
