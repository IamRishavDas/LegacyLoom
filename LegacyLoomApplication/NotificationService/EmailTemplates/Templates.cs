using System.ComponentModel.DataAnnotations;

namespace NotificationService.EmailTemplates
{
    public enum TemplateName
    {
        WELCOME
    }
    public class Templates
    {
        public string GetTemplate([Required]TemplateName templateName, string? userName)
        {
            List<string> templateContents = new()
            {
                #region WELCOME Email
                $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Welcome to Legacy Loom</title>
                    <style>
                        body {{
                            margin: 0;
                            padding: 0;
                            font-family: 'Georgia', 'Times New Roman', serif;
                            background: #f5f7fa;
                            color: #333;
                        }}
                        .email-container {{
                            max-width: 600px;
                            width: 100%;
                            background: #fff;
                            border-radius: 8px;
                            margin: 20px auto;
                            border: 1px solid #ddd;
                        }}
                        .header {{
                            padding: 30px;
                            text-align: center;
                            background: #8b4513;
                            color: #fff;
                        }}
                        .logo-container {{
                            display: inline-block;
                        }}
                        .brand-name {{
                            font-size: 24px;
                            font-weight: bold;
                            margin-bottom: 5px;
                        }}
                        .tagline {{
                            font-size: 12px;
                            text-transform: uppercase;
                            letter-spacing: 1px;
                            opacity: 0.8;
                        }}
                        .hero-image {{
                            width: 100%;
                            height: 250px;
                            object-fit: cover;
                            display: block;
                        }}
                        .hero-text {{
                            position: absolute;
                            bottom: 15px;
                            left: 20px;
                            color: #fff;
                            text-shadow: 1px 1px 2px rgba(0,0,0,0.5);
                        }}
                        .hero-title {{
                            font-size: 16px;
                            font-weight: bold;
                        }}
                        .hero-subtitle {{
                            font-size: 12px;
                        }}
                        .content {{
                            padding: 30px;
                        }}
                        .main-title {{
                            font-size: 28px;
                            color: #2c1810;
                            text-align: center;
                            margin: 0 0 20px;
                        }}
                        .main-title::after {{
                            content: '';
                            display: block;
                            width: 50px;
                            height: 2px;
                            background: #d2691e;
                            margin: 10px auto;
                        }}
                        .welcome-box {{
                            padding: 20px;
                            border-left: 3px solid #d2691e;
                            background: #f9f9f9;
                            border-radius: 4px;
                            margin-bottom: 20px;
                        }}
                        .welcome-text {{
                            font-size: 16px;
                            line-height: 1.6;
                            color: #333;
                            margin: 0 0 10px;
                        }}
                        .welcome-subtext {{
                            font-size: 14px;
                            line-height: 1.6;
                            color: #555;
                        }}
                        .feature-item {{
                            display: flex;
                            align-items: center;
                            margin-bottom: 15px;
                            padding: 15px;
                            background: #fff;
                            border: 1px solid #eee;
                            border-radius: 4px;
                        }}
                        .feature-icon {{
                            width: 40px;
                            height: 40px;
                            border-radius: 50%;
                            display: flex;
                            align-items: center;
                            justify-content: center;
                            margin-right: 15px;
                            color: #fff;
                            font-size: 18px;
                        }}
                        .feature-icon-1 {{ background: #ff6b6b; }}
                        .feature-icon-2 {{ background: #4ecdc4; }}
                        .feature-icon-3 {{ background: #45b7d1; }}
                        .feature-title {{
                            font-weight: bold;
                            color: #2c1810;
                            margin-bottom: 5px;
                        }}
                        .feature-desc {{
                            color: #666;
                            font-size: 13px;
                        }}
                        .cta-button {{
                            display: inline-block;
                            padding: 12px 30px;
                            font-size: 16px;
                            color: #fff;
                            text-decoration: none;
                            border-radius: 25px;
                            background: #d2691e;
                            font-weight: bold;
                        }}
                        .support-section {{
                            text-align: center;
                            padding: 20px;
                            background: #f9f9f9;
                            border-radius: 4px;
                            margin-top: 20px;
                        }}
                        .support-section strong {{
                            font-size: 14px;
                            color: #2c1810;
                            margin-bottom: 10px;
                            display: block;
                        }}
                        .support-section p {{
                            font-size: 13px;
                            line-height: 1.5;
                            color: #666;
                            margin: 0;
                        }}
                        .support-section a {{
                            color: #d2691e;
                            text-decoration: none;
                            font-weight: bold;
                        }}
                        .footer {{
                            padding: 20px;
                            background: #2c1810;
                            text-align: center;
                            color: #fff;
                        }}
                        .footer div {{
                            font-size: 14px;
                            font-weight: bold;
                            margin-bottom: 5px;
                        }}
                        .footer p {{
                            font-size: 12px;
                            opacity: 0.8;
                            margin: 10px 0 0;
                        }}

                        /* Media Queries */
                        @media only screen and (max-width: 600px) {{
                            .email-container {{ width: 100%; margin: 0; }}
                            .content {{ padding: 20px; }}
                            .header {{ padding: 20px; }}
                            .brand-name {{ font-size: 20px; }}
                            .tagline {{ font-size: 10px; }}
                            .hero-text {{ left: 15px; }}
                            .hero-title {{ font-size: 14px; }}
                            .hero-subtitle {{ font-size: 11px; }}
                            .main-title {{ font-size: 24px; }}
                            .welcome-box {{ padding: 15px; }}
                            .welcome-text {{ font-size: 15px; }}
                            .welcome-subtext {{ font-size: 13px; }}
                            .feature-item {{ flex-direction: column; text-align: center; padding: 10px; }}
                            .feature-icon {{ margin: 0 0 10px; }}
                            .cta-button {{ padding: 10px 20px; font-size: 14px; }}
                            .support-section {{ padding: 15px; }}
                            .footer {{ padding: 15px; }}
                        }}
                        @media only screen and (max-width: 480px) {{
                            .main-title {{ font-size: 20px; }}
                            .welcome-text {{ font-size: 14px; }}
                            .welcome-subtext {{ font-size: 12px; }}
                            .hero-image {{ height: 200px; }}
                            .feature-item {{ padding: 8px; }}
                            .feature-title {{ font-size: 13px; }}
                            .feature-desc {{ font-size: 12px; }}
                        }}
                    </style>
                </head>
                <body>
                    <table width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td align=""center"">
                                <table class=""email-container"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                                    <!-- Header -->
                                    <tr>
                                        <td class=""header"">
                                            <div class=""logo-container"">
                                                <div class=""brand-name"">Legacy Loom</div>
                                                <div class=""tagline"">Weaving Stories • Crafting Memories</div>
                                            </div>
                                        </td>
                                    </tr>
                                    <!-- Hero Image Section -->
                                    <tr>
                                        <td style=""position: relative;"">
                                            <img class=""hero-image"" src=""https://images.unsplash.com/photo-1528569937393-ee892b976859?q=80&w=2070&auto=format&fit=crop"" alt=""Artisan weaving on traditional loom"">
                                            <div class=""hero-text"">
                                                <div class=""hero-title"">Craftsmanship Meets Digital Innovation</div>
                                                <div class=""hero-subtitle"">Where traditions are preserved and stories come alive</div>
                                            </div>
                                        </td>
                                    </tr>
                                    <!-- Main Content -->
                                    <tr>
                                        <td class=""content"">
                                            <h1 class=""main-title"">Welcome to Your Creative Journey!</h1>
                                            <div class=""welcome-box"">
                                                <p class=""welcome-text"">Hello <strong style=""color: #d2691e;"">{userName}</strong>,</p>
                                                <p class=""welcome-subtext"">
                                                    Welcome to Legacy Loom, where digital innovation meets timeless craftsmanship. You're now part of a community that celebrates the art of storytelling through weaving, preserving traditions while creating new memories that will last for generations.
                                                </p>
                                            </div>
                                            <!-- Feature Highlights -->
                                            <div style=""margin: 30px 0;"">
                                                <div class=""feature-item"">
                                                    <div class=""feature-icon feature-icon-1"">🧵</div>
                                                    <div>
                                                        <div class=""feature-title"">Craft Your Stories</div>
                                                        <div class=""feature-desc"">Transform your experiences into beautiful, woven narratives</div>
                                                    </div>
                                                </div>
                                                <div class=""feature-item"">
                                                    <div class=""feature-icon feature-icon-2"">🏛️</div>
                                                    <div>
                                                        <div class=""feature-title"">Preserve Traditions</div>
                                                        <div class=""feature-desc"">Keep family heritage and cultural practices alive for future generations</div>
                                                    </div>
                                                </div>
                                                <div class=""feature-item"">
                                                    <div class=""feature-icon feature-icon-3"">🤝</div>
                                                    <div>
                                                        <div class=""feature-title"">Connect & Share</div>
                                                        <div class=""feature-desc"">Join a community of creators and storytellers worldwide</div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- CTA Button -->
                                            <div style=""text-align: center; margin: 30px 0;"">
                                                <a class=""cta-button"" href=""https://example.com/get-started"" target=""_blank"">Begin Your Legacy</a>
                                            </div>
                                            <!-- Support Section -->
                                            <div class=""support-section"">
                                                <strong>Need assistance?</strong>
                                                <p>Our support team is here to help.<br>
                                                Reach out at <a href=""mailto:legacyloomapp@gmail.com"">legacyloomapp@gmail.com</a></p>
                                            </div>
                                        </td>
                                    </tr>
                                    <!-- Footer -->
                                    <tr>
                                        <td class=""footer"">
                                            <div>Legacy Loom</div>
                                            <p>© 2025 Legacy Loom. All rights reserved.</p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>
                "    
                #endregion
            };
            return templateContents[(int)templateName];
        }
        
    }
}
