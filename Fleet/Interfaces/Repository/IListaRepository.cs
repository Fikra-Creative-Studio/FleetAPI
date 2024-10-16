﻿using Fleet.Enums;
using Fleet.Models;

namespace Fleet.Interfaces.Repository
{
    public interface IListaRepository : IBaseWorkspaceRepository<Listas>
    {
        List<Listas> BuscarComItems(int workspaceId);
        void TornarPadrao(int workspaceId, int listaId);
        bool Existe(int workspaceId, TipoListasEnum tipo);
    }
}
