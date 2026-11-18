import { Controller } from '@nestjs/common';
import { DifusionesService } from './difusiones.service';

@Controller('difusiones')
export class DifusionesController {
  constructor(private readonly difusionesService: DifusionesService) {}
}
