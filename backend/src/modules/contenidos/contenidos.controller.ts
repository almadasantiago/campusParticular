import { Controller } from '@nestjs/common';
import { ContenidosService } from './contenidos.service';

@Controller('contenidos')
export class ContenidosController {
  constructor(private readonly contenidosService: ContenidosService) {}
}
