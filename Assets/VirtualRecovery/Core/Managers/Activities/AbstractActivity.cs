// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 08/07/2025
//  */

using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers.Activities {
    internal abstract class AbstractActivity {
        public abstract void Load(DifficultyLevel difficultyLevel, BodySide bodySide);
        protected abstract void LoadEasy(BodySide bodySide);
        protected abstract void LoadMedium(BodySide bodySide);
        protected abstract void LoadHard(BodySide bodySide);
    }
}