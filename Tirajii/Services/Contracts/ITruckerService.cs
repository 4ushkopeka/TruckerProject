﻿using Tirajii.Models.Trucker;

namespace Tirajii.Services.Contracts
{
    public interface ITruckerService
    {
        Task RegisterTrucker(TruckerRegisterViewModel model, string userId);
    }
}
