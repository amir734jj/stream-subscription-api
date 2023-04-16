﻿using EfCoreRepository.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models.Models;

namespace Logic.Crud;

public class UserLogic : BasicLogicAbstract<User>, IUserLogic
{
    private readonly IBasicCrud<User> _userDal;

    /// <summary>
    /// Constructor dependency injection
    /// </summary>
    /// <param name="userDal"></param>
    public UserLogic(IBasicCrud<User> userDal)
    {
        _userDal = userDal;
    }

    /// <summary>
    /// Returns DAL
    /// </summary>
    /// <returns></returns>
    protected override IBasicCrud<User> GetBasicCrudDal()
    {
        return _userDal;
    }
}