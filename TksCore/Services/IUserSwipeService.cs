using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;

namespace Tks.Services
{
    public interface IUserSwipeService
        :IEntityService
    {
        List<UserSwipe> Retrieve(int userId, DateTime fromDate, DateTime toDate);
        void CheckIn(UserSwipe entity);
        void CheckOut(UserSwipe entity);
        void RemoveUserSwipeCheckInOutDetailsint(int userId, DateTime workDate, int movedUserId, string movedreason);

    }
}
