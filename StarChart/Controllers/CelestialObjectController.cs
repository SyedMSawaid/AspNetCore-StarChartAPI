using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var results = _context.CelestialObjects.ToList();
            foreach (var result in results)
            {
                result.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == result.Id).ToList();
            }

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObjects = _context.CelestialObjects.Find(id);
            if (celestialObjects == null)
            {
                return NotFound();
            }

            celestialObjects.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();
            return Ok(celestialObjects);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.FirstOrDefault(c => c.Name == name);
            if (result == null)
            {
                return NotFound();
            }
            result.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == result.Id).ToList();
            return Ok(result);
        }
    }
}
