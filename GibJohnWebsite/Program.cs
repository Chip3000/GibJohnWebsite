using GibJohnWebsite.Data;
using GibJohnWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GibJohnWebsiteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GibJohnWebsiteContext") ?? throw new InvalidOperationException("Connection string 'GibJohnWebsiteContext' not found.")));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Ensure role management is added here
    .AddEntityFrameworkStores<ApplicationDbContext>(); // Ensure this uses ApplicationDbContext
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Create roles and seed courses
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RemoveOldLessons(services);
    await SeedRoles(services);
    await SeedCourses(services);
    await SeedTutors(services);
    await SeedLessons(services);
}

app.Run();

async Task SeedRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = { "Admin", "Tutor", "Student"};
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Create the roles and seed them to the database
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

async Task SeedCourses(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<GibJohnWebsiteContext>();

    // Check if any courses exist
    if (!context.CoursesClass.Any())
    {
        // Add initial courses
        var courses = new List<CoursesClass>
        {
            new CoursesClass { Title = "English Literature", Description = "This course offers a comprehensive exploration of English literature from its origins to contemporary works. Students will engage with a diverse range of texts, including poetry, drama, and prose, while developing critical thinking and analytical skills. The course emphasizes the historical, cultural, and social contexts of literary works and encourages students to articulate their interpretations through writing and discussion."},
            new CoursesClass { Title = "Mathematics", Description = "This course serves as a comprehensive introduction to fundamental mathematical concepts and techniques. Students will explore a variety of topics, including algebra, geometry, statistics, and introductory calculus. The course emphasizes problem-solving strategies, logical reasoning, and the application of mathematical principles to real-world situations. Through a blend of theoretical instruction and practical exercises, students will develop a strong mathematical foundation that prepares them for advanced studies."},
            new CoursesClass { Title = "Science: Biology", Description = "This course offers a foundational understanding of biological principles and processes that govern living organisms. Students will explore various topics, including cell biology, genetics, evolution, ecology, and human anatomy. Through a combination of lectures, laboratory experiments, and field studies, students will develop critical thinking skills, scientific literacy, and an appreciation for the diversity of life."},
            new CoursesClass { Title = "Science: Chemistry", Description = "This course provides an essential foundation in chemistry, exploring the composition, structure, properties, and changes of matter. Students will engage with various topics, including atomic theory, chemical bonding, stoichiometry, thermodynamics, and organic chemistry. Through a combination of lectures, hands-on laboratory experiments, and problem-solving exercises, students will develop critical thinking skills and an appreciation for the role of chemistry in everyday life and various scientific fields."},
            new CoursesClass { Title = "Science: Physics", Description = "This course offers a foundational understanding of the principles governing the physical universe. Students will explore a variety of topics, including mechanics, thermodynamics, waves, electricity, magnetism, and modern physics. Through a combination of lectures, laboratory experiments, and problem-solving exercises, students will develop critical thinking skills and an appreciation for the role of physics in everyday life and technological advancements."},
            new CoursesClass { Title = "Psychology", Description = "This course provides an overview of the scientific study of behavior and mental processes. Students will explore various topics, including the biological basis of behavior, cognitive processes, development, personality, social psychology, and psychological disorders. Through lectures, discussions, and experiential learning, students will develop a deeper understanding of psychological concepts and their relevance to everyday life."},
            new CoursesClass { Title = "Philosophy", Description = "This course provides an overview of key philosophical questions, theories, and thinkers throughout history. Students will explore topics such as ethics, metaphysics, epistemology, political philosophy, and the philosophy of mind. Through readings, discussions, and writing assignments, students will develop critical thinking skills, engage with complex ideas, and learn to articulate their own philosophical positions."},
            new CoursesClass { Title = "IT: Software Development", Description = "This course provides an overview of the software development lifecycle, from initial concept through design, implementation, testing, and maintenance. Students will learn fundamental programming concepts, software design principles, and project management techniques. Through hands-on projects and collaborative work, students will develop practical skills and gain experience in building software applications."},
            new CoursesClass { Title = "IT: Cyber Security", Description = "This course provides an overview of the principles and practices of cybersecurity, focusing on protecting computer systems, networks, and data from cyber threats. Students will learn about various types of cyber threats, security measures, and best practices for maintaining cybersecurity. Through hands-on labs and projects, students will gain practical experience in identifying vulnerabilities and implementing security solutions."},
            new CoursesClass { Title = "Geography", Description = "This course offers a comprehensive exploration of the Earth's landscapes, environments, and the relationships between people and their environments. Students will delve into both physical and human geography, examining the natural processes that shape our planet, such as climate, landforms, and ecosystems, as well as the cultural, economic, and political factors that influence human activity and settlement patterns."},
            new CoursesClass { Title = "History", Description = "This course provides a foundational understanding of key historical events, movements, and figures that have shaped human civilization from ancient times to the present. Students will engage with diverse perspectives and methodologies to analyze historical narratives, develop critical thinking skills, and gain insights into the complexities of the past."},
            new CoursesClass { Title = "Sociology", Description = "This course serves as an introduction to the study of sociology, the systematic exploration of society, social behavior, and social institutions. Students will examine the ways in which social structures, cultural norms, and individual actions intersect to shape human experiences and societal outcomes."},
            new CoursesClass { Title = "Foreign Languages", Description = "This course offers an engaging introduction to the study of foreign languages, emphasizing the importance of language as a tool for communication, cultural exchange, and understanding in our increasingly interconnected world. Students will explore the basics of language acquisition, linguistic structures, and the cultural contexts that shape language use."},
            new CoursesClass { Title = "Graphic Design", Description = "This course provides an overview of graphic design principles, tools, and practices. Students will explore the visual communication process, learning how to effectively convey messages through design. The course emphasizes creativity, technical skills, and an understanding of design theory."},
            new CoursesClass { Title = "Fitness Coaching", Description = "This course provides a comprehensive overview of fitness coaching, focusing on the principles of exercise science, program design, and client motivation. Students will learn the skills necessary to help individuals achieve their fitness goals, whether they are seeking weight loss, muscle gain, improved athletic performance, or overall health and wellness."},
            new CoursesClass { Title = "Astrology", Description = "This course offers an in-depth exploration of astrology, a practice that examines the influence of celestial bodies on human behavior and events. Students will learn about the foundations of astrology, including its history, key concepts, and various systems of interpretation. The course aims to provide a balanced understanding of astrology as both an art and a science."},
            new CoursesClass { Title = "Architecture", Description = "This course provides a foundational understanding of architecture as both an art and a science. Students will explore the principles of design, the history of architectural styles, and the role of architecture in society. The course emphasizes critical thinking, creativity, and the technical skills necessary for architectural practice."},
            new CoursesClass { Title = "Fashion Design", Description = "This course offers a comprehensive overview of the fashion design process, from concept development to the final product. Students will explore the history of fashion, the elements and principles of design, and the technical skills necessary to create original garments and collections. The course encourages creativity and individual expression while providing a solid foundation in the practical aspects of fashion design."}
        };

        context.CoursesClass.AddRange(courses);
        await context.SaveChangesAsync();
    }
}

async Task SeedTutors(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<GibJohnWebsiteContext>();

    // Check if any courses exist
    if (!context.TutorsClass.Any())
    {
        // Add initial courses
        var tutors = new List<TutorsClass>
        {
            new TutorsClass { Name = "Luke Thompson", Email = "luke@macss.co.uk", PhoneNumber = "+77 392 341406", Subject = "Astrology"},
            new TutorsClass { Name = "Kieran Edgeworth", Email = "ohimdead@example.com", PhoneNumber = "+77 819 432756", Subject = "Fitness Coaching"},
            new TutorsClass { Name = "Julie Rosier", Email = "julierosier@example.com", PhoneNumber = "+77 123 456789", Subject = "IT: Software Development & IT: Cyber Security"},
            new TutorsClass { Name = "Brandon White", Email = "brandonwhite@example.com", PhoneNumber = "+77 987 654321", Subject = "Fashion Design"}
        };

        context.TutorsClass.AddRange(tutors);
        await context.SaveChangesAsync();
    }
}

async Task SeedLessons(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<GibJohnWebsiteContext>();

    // Check if any courses exist
    if (!context.AddLessonClass.Any())
    {
        // Add initial courses
        var lessons = new List<AddLessonClass>
        {
            new AddLessonClass { Title = "Astrology", Description = "In this lesson you will be learning about how the age of a star alters a solar system.", Time = DateTime.Now.AddDays(1), Tutor = "Luke Thompson"},
            new AddLessonClass { Title = "Fitness Coaching", Description = "In this lesson you will learn how beats per minute (bpm) allows us to carry out endurance activities.", Time = DateTime.Now.AddDays(2), Tutor = "Kieran Edgeworth"},
            new AddLessonClass { Title = "Software Development", Description = "In this lesson you will learn how to create a basic website with a login and sign up system.", Time = DateTime.Now.AddDays(3), Tutor = "Julie Rosier"},
            new AddLessonClass { Title = "Fashion Design", Description = "In this lesson we will be virtually designing a prom dress from a clients brief.", Time = DateTime.Now.AddDays(4), Tutor = "Brandon White"}
        };

        context.AddLessonClass.AddRange(lessons);
        await context.SaveChangesAsync();
    }
}

async Task RemoveOldLessons(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<GibJohnWebsiteContext>();

    // Find lessons where the Time is less than the current time
    var oldLessons = await context.AddLessonClass
        .Where(lesson => lesson.Time < DateTime.Now)
        .ToListAsync();

    if (oldLessons.Any())
    {
        context.AddLessonClass.RemoveRange(oldLessons);
        await context.SaveChangesAsync();
    }
}