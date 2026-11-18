import { Usuario as UsuarioPrisma } from 'generated\prisma\models\Usuario.ts';
import { Rol } from '../../generated/prisma/enums';


enum Rol {
  ADMIN
  PROFESOR
  ESTUDIANTE
}

model Usuario {
  id           String   @id @default(uuid())
  email        String   @unique
  passwordHash String
  nombre       String
  apellido     String
  rol          Rol
  activo       Boolean  @default(true)
  createdAt    DateTime @default(now())
  updatedAt    DateTime @updatedAt
} 