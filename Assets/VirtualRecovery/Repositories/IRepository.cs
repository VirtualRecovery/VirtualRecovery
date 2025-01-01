// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 31/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery.Repositories {
    internal interface IRepository<T> {
        void Insert(T entity);
        void Update(int id, T entity);
        void Delete(int id);
        T GetById(int id);
        List<T> GetAll();
    }

}