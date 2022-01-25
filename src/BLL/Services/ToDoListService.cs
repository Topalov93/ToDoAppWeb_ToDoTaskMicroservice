using Common;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Models.Users;

namespace ToDoApp.Services.ToDoListService
{
    public class ToDoListService : IToDoListService
    {
        private readonly ToDoListsRepository _toDoListRepository;

        public ToDoListService(ToDoListsRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }

        public async Task<ResultState> CreateToDoList(ToDoList newToDoList, int currentUserId)
        {
            ToDoList toDoList = await _toDoListRepository.GetToDoListByTitle(newToDoList.Title);

            if (toDoList is not null)
            {
                return new ResultState(false, Messages.ToDoListAlreadyExist);
            }

            newToDoList.UserId = currentUserId;

            try
            {
                await _toDoListRepository.CreateToDoList(newToDoList);
                return new ResultState(true, Messages.ToDoListCreationSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.ToDoListUnableToCreate, ex);
            }
        }

        public async Task<ResultState> DeleteToDoList(int toDoListToDeleteId, int currentUserId)
        {
            ToDoList toDoListToShare = await _toDoListRepository.GetToDoListById(toDoListToDeleteId);

            if (toDoListToShare is null)
            {
                return new ResultState(false, Messages.ToDoListDoesntExist);
            }

            if (currentUserId != toDoListToShare.UserId)
            {
                List<int> userShared = await _toDoListRepository.GetUsersSharedToToDoList(toDoListToDeleteId);

                if (userShared.Contains(currentUserId))
                {
                    try
                    {
                        await _toDoListRepository.RemoveSharing(toDoListToDeleteId, currentUserId);
                        return new ResultState(true, Messages.ToDoListSharingRemoved);

                    }
                    catch (Exception ex)
                    {
                        return new ResultState(false, Messages.ToDoListUnableToRemoveSharing, ex);
                    }
                }

                return new ResultState(false, Messages.UserDontHaveRightsToDeleteToDoList);
            }

            try
            {
                await _toDoListRepository.DeleteToDoList(toDoListToDeleteId);
                return new ResultState(true, Messages.ToDoListDeletedSuccessfull);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.ToDoListUnableToDelete, ex);
            }
        }

        public async Task<ResultState> EditToDoList(int toDoListId, ToDoList newInfoHolderToDoList, int currentUserId)
        {
            ToDoList toDoListToEdit = await _toDoListRepository.GetToDoListById(toDoListId);

            if (toDoListToEdit is null)
            {
                return new ResultState(false, Messages.ToDoListDoesntExist);
            }

            newInfoHolderToDoList.EditedBy = currentUserId;
            newInfoHolderToDoList.EditedOn = DateTime.UtcNow;

            List<int> userShared = await _toDoListRepository.GetUsersSharedToToDoList(toDoListId);

            if (currentUserId == toDoListToEdit.UserId || userShared.Contains(currentUserId))
            {
                try
                {
                    await _toDoListRepository.EditToDoList(toDoListId, newInfoHolderToDoList);
                    return new ResultState(true, Messages.ToDoListEditSuccessfull);
                }

                catch (Exception ex)
                {
                    return new ResultState(false, Messages.ToDoListUnableToEdit, ex);
                }
            }

            return new ResultState(false, Messages.UserDontHaveRightsToEditToDoList);
        }

        public async Task<ResultState> ShareToDoList(int currentUserId, int toDoListId, int userId, List<User> users)
        {
            ToDoList toDoListToShare = await _toDoListRepository.GetToDoListById(toDoListId);

            if (toDoListToShare is null)
            {
                return new ResultState(false, Messages.ToDoListDoesntExist);
            }

            if (!users.Any(u => u.Id == userId))
            {
                return new ResultState(false, Messages.UserDoesntExist);
            }

            if (toDoListToShare.UserId != currentUserId)
            {
                return new ResultState(false, Messages.UserDontHaveRightsToShareToDoList);
            }

            var usersShared = await _toDoListRepository.GetUsersSharedToToDoList(toDoListId);

            if (toDoListToShare.UserId == userId || usersShared.Contains(userId))
            {
                return new ResultState(false, Messages.UserAlreadyHaveShareToToDoListOrIsCreator);
            }

            try
            {
                await _toDoListRepository.ShareToDoList(toDoListId, userId);
                return new ResultState(true, Messages.ToDoListSharingSuccessful);
            }
            catch (Exception ex)
            {
                return new ResultState(false, Messages.ToDoListUnableToShare, ex);
            }
        }

        public async Task<List<ToDoList>> ListDoToLists()
        {
            return await _toDoListRepository.GetToDoLists();
        }

        public async Task<ToDoList> GetToDoListById(int listId)
        {
            return await _toDoListRepository.GetToDoListById(listId);
        }

        public async Task<List<int>> UsersSharedToToDoList(int listId)
        {
            return await _toDoListRepository.GetUsersSharedToToDoList(listId);
        }

        public async Task<ToDoList> GetToDoListByToDoTaskId(int toDoTaskId)
        {
            return await _toDoListRepository.GetToDoListByToDoTaskId(toDoTaskId);
        }
    }
}
