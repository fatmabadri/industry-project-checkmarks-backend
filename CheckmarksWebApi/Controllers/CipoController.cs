using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheckmarksWebApi.Models;
using Microsoft.AspNetCore.Cors;
using CheckmarksWebApi.ViewModels;
using Microsoft.Extensions.Logging;

namespace CheckmarksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy")]
    public class CipoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ILogger<CipoController> _logger;

        public CipoController(ApplicationDbContext context, ILogger<CipoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/cipo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NICEClass>>> GetNICEClasses()
        {
            return await _context.NICEClasses.ToListAsync();
        }

        // GET: api/cipo/GetAllClasses
        [HttpGet]
        [Route("GetAllClasses")]
        public async Task<ActionResult<ClassList>> GetAll()
        {
            var nicecl = await _context.NICEClasses.ToArrayAsync();

            ClassList cl = new ClassList()
            {
                Classes = new ClassJson[nicecl.Length]
            };

            for (int i = 0; i < nicecl.Length; i++)
            {
                ClassJson cj = new ClassJson()
                {
                    Id = nicecl[i].Id,
                    Name = nicecl[i].ShortName
                };
                cl.Classes[i] = cj;
            }

            return cl;
        }

        // GET: api/cipo/GetTerms/1
        [HttpGet]
        [Route("GetTerms/{id}")]
        public async Task<ActionResult<TermList>> GetTermsById(int id)
        {
            // use ef where-clause
            var termsById = await _context.NICETerms.Where(t => t.ClassId == id).ToArrayAsync();

            var termlist = new TermList()
            {
                Terms = new Term[termsById.Length]
            };

            for (int i = 0; i < termsById.Length; i++)
            {
                Term t = new Term()
                {
                    Id = termsById[i].Id,
                    TermName = termsById[i].Name
                };

                termlist.Terms[i] = t;
            }

            return termlist;
        }

        // GET: api/cipo/GetTermsByString/str
        [HttpGet]
        [Route("GetTermsByString/{str}")]
        public async Task<ActionResult<TermList>> GetTermsByStringMatch(string str) {

            _logger.LogInformation($"[api/cipo] {DateTime.Now} - Searched for term {str} .");
            var termsByString = await _context.NICETerms.Where(t => t.Name.Contains(str)).ToArrayAsync();
            _logger.LogInformation($"[api/cipo] {DateTime.Now} - Search returned {termsByString.Length} terms.");

            var termlist = new TermList() {
                Terms = new Term[termsByString.Length]
            };


            for (int i=0; i<termsByString.Length; i++) {
                Term t = new Term() {
                    Id = termsByString[i].Id,
                    TermName = termsByString[i].Name
                };

                termlist.Terms[i] = t;
            }

            return termlist;
        }

        // GET: api/cipo/GetTermsByString/str
        [HttpGet]
        [Route("GetTermDataByString/{str}")]
        public async Task<ActionResult<NewTermList>> GetTermDataByStringMatch(string str)
        {
            _logger.LogInformation($"[api/cipo] {DateTime.Now} - Searched for term {str} .");
            var termsByString = await _context.NICETerms.Where(t => t.Name.Contains(str)).ToArrayAsync();
            var nicecl = await _context.NICEClasses.ToArrayAsync();
            var sortedList = nicecl.OrderBy(si => si.Id).ToArray();
            _logger.LogInformation($"[api/cipo] {DateTime.Now} - Search returned {termsByString.Length} terms.");

            var termlist = new NewTermList()
            {
                Terms = new NewTerm[termsByString.Length]
            };


            for (int i = 0; i < termsByString.Length; i++)
            {
                int classID = termsByString[i].ClassId;
                NewTerm t = new NewTerm()
                {
                    Id = termsByString[i].Id,
                    TermName = termsByString[i].Name,
                    TermClass = classID,
                    ClassShortName = sortedList[classID -1].ShortName
                };

                termlist.Terms[i] = t;
            }

            return termlist;
        }

        // GET: api/cipo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NICEClass>> GetNICEClass(int id)
        {
            var nICEClass = await _context.NICEClasses.FindAsync(id);

            if (nICEClass == null)
            {
                return NotFound();
            }

            return nICEClass;
        }

        // PUT: api/cipo/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutNICEClass(int id, NICEClass nICEClass)
        // {
        //     if (id != nICEClass.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(nICEClass).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!NICEClassExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // POST: api/cipo
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        // [HttpPost]
        // public async Task<ActionResult<NICEClass>> PostNICEClass(NICEClass nICEClass)
        // {
        //     _context.NICEClasses.Add(nICEClass);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetNICEClass", new { id = nICEClass.Id }, nICEClass);
        // }

        // DELETE: api/cipo/5
        // [HttpDelete("{id}")]
        // public async Task<ActionResult<NICEClass>> DeleteNICEClass(int id)
        // {
        //     var nICEClass = await _context.NICEClasses.FindAsync(id);
        //     if (nICEClass == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.NICEClasses.Remove(nICEClass);
        //     await _context.SaveChangesAsync();

        //     return nICEClass;
        // }

        private bool NICEClassExists(int id)
        {
            return _context.NICEClasses.Any(e => e.Id == id);
        }
    }
}
