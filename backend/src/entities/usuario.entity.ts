import type { Rol } from '../../generated/prisma/enums.js';

export class Usuario {
  id: string;
  email: string;
  passwordHash: string;
  nombre: string;
  apellido: string;
  rol: Rol;
  activo: boolean;
  createdAt: Date;
  updatedAt: Date;
}