import { Controller } from '@nestjs/common';
import { EntregasService } from './entregas.service.js';

@Controller('entregas')
export class EntregasController {
  constructor(private readonly entregasService: EntregasService) {}
}
